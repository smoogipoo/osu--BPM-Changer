using BMAPI.v1;
using BMAPI.v1.Events;
using BMAPI.v1.HitObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace osu_trainer
{

    enum EditorState
    {
        NOT_READY,
        READY,
        GENERATING_BEATMAP
    }

    enum BadBeatmapReason
    {
        NO_BEATMAP_LOADED,
        ERROR_LOADING_BEATMAP,
        DIFF_NOT_OSUSTD
    }

    // note: this code suffers from possible race conditions due to async functions modified shared resources (OriginalBeatmap, NewBeatmap)
    // however, enough mechanisms are put in place so that this doesn't really happen in during real usage
    // possible race condition: 
    //
    //    user changes bpm:                              user selects another beatmap:
    //       NewBeatmap.TimingPoints are modified           NewBeatmap changes
    //       

    class BeatmapEditor
    {
        MainForm form;
        public BadBeatmapReason NotReadyReason;

        public Beatmap OriginalBeatmap;
        public Beatmap NewBeatmap;

        public float StarRating { get; set; }
        public float AimRating {get; set;}
        public float SpeedRating {get; set;}
        float lockedHP = 0f;
        float lockedCS = 0f;
        float lockedAR = 0f;
        float lockedOD = 0f;

        class ConcurrentRequest
        {
            static int globalRequestCounter = -1;
            public int RequestNumber { get; set; }

            public ConcurrentRequest()
            {
                RequestNumber = ++globalRequestCounter;
            }
        }
        class BeatmapRequest : ConcurrentRequest
        {
            public string Name { get; set; }
            public BeatmapRequest(string name) : base()
            {
                Name = name;
            }
        }
        private List<BeatmapRequest> mapChangeRequests = new List<BeatmapRequest>();
        BeatmapRequest    completedBeatmapRequest      = null;
        ConcurrentRequest completedDiffCalcRequest     = null;
        bool serviceBeatmapRequestLocked               = false;  // mutex for serviceBeatmapChangeRequest()
        bool serviceDiffCalcRequestLocked              = false;  // mutex for serviceDiffCalcRequest()
        private List<ConcurrentRequest> diffCalcRequests = new List<ConcurrentRequest>();

        // public getters only
        // to set, call set methods
        public bool HpIsLocked { get; private set; } = false;
        public bool CsIsLocked { get; private set; } = false;
        public bool ArIsLocked { get; private set; } = false;
        public bool OdIsLocked { get; private set; } = false;
        public bool ScaleAR { get; private set; } = true;
        public bool ScaleOD { get; private set; } = true;
        internal EditorState State { get; private set; }
        public float BpmMultiplier { get; set; } = 1.0f;

        public BeatmapEditor(MainForm f)
        {
            form = f;
            setState(EditorState.NOT_READY);
            NotReadyReason = BadBeatmapReason.NO_BEATMAP_LOADED;
        }

        public event EventHandler StateChanged;
        public event EventHandler BeatmapSwitched;
        public event EventHandler BeatmapModified;
        public event EventHandler ControlsModified;

        public void ForceEventStateChanged()     => StateChanged?.Invoke(this, EventArgs.Empty);
        public void ForceEventBeatmapSwitched()  => BeatmapSwitched?.Invoke(this, EventArgs.Empty);
        public void ForceEventBeatmapModified()  => BeatmapModified?.Invoke(this, EventArgs.Empty);
        public void ForceEventControlsModified() => ControlsModified?.Invoke(this, EventArgs.Empty);

        public async void GenerateBeatmap()
        {
            if (State != EditorState.READY)
                return;

            // pre
            setState(EditorState.GENERATING_BEATMAP);

            // main phase
            Beatmap exportBeatmap = NewBeatmap.Copy();
            ModifyBeatmapMetadata(exportBeatmap, BpmMultiplier);
            if (!File.Exists(JunUtils.GetBeatmapDirectoryName(OriginalBeatmap) + "\\" + exportBeatmap.AudioFilename))
            {
                await Task.Run(() => SongSpeedChanger.GenerateAudioFile(OriginalBeatmap, exportBeatmap, BpmMultiplier));

                // take note of this mp3 in a text file, so we can clean it up later
                string mp3ManifestFile = Properties.Settings.Default.SongsFolder + "\\modified_mp3_list.txt";
                using (var writer = File.AppendText(mp3ManifestFile))
                {
                    string beatmapFolder = Path.GetDirectoryName(exportBeatmap.Filename).Replace(Properties.Settings.Default.SongsFolder + "\\", "");
                    string mp3RelativePath = beatmapFolder + "\\" + exportBeatmap.AudioFilename;
                    writer.WriteLine(mp3RelativePath + " | " + exportBeatmap.Filename);
                }
            }
            exportBeatmap.Save();

            // post
            form.PlayDoneSound();
            setState(EditorState.READY);
        }

        public List<string> GetUnusedMp3s()
        {
            // read manifest file
            List<string> lines = new List<string>();
            string mp3ManifestFile = Properties.Settings.Default.SongsFolder + "\\modified_mp3_list.txt";

            if (!File.Exists(mp3ManifestFile))
                return new List<string>();

            using (var reader = File.OpenText(mp3ManifestFile))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }

            // convert that shit into a dictionary
            var mp3Dict = new Dictionary<string, List<string>>();
            string pattern = @"(.+) \| (.+)";
            string matchGroup(string text, string re, int group)
            {
                foreach (Match m in Regex.Matches(text, re))
                    return m.Groups[group].Value;
                return "";
            }
            string parseMp3(string line) => matchGroup(line, pattern, 1);
            string parseOsu(string line) => matchGroup(line, pattern, 2);

            // create dictionary keys
            lines
                .Select(line => parseMp3(line)).ToList()
                .ForEach(mp3 => mp3Dict.Add(mp3, new List<string>()));

            // populate dictionary values
            foreach ((string mp3, string osu) in lines.Select(line => (parseMp3(line), parseOsu(line))))
                mp3Dict[mp3].Add(osu);

            // find all keys where none of the associated beatmaps exist
            bool noFilesExist(bool acc, string file) => acc && !File.Exists(file);
            return lines
                .Select(line => parseMp3(line))
                .Where(
                    mp3 => mp3Dict[mp3].Aggregate(true, noFilesExist)
                ).ToList();
        }

        // TODO: simulate long load time and test
        // TODO: set update interval to be really large and test
        public void RequestBeatmapLoad(string beatmapPath)
        {
            mapChangeRequests.Add(new BeatmapRequest(beatmapPath));

            // acquire mutually exclusive entry into this method
            if (!serviceBeatmapRequestLocked)
                serviceBeatmapChangeRequest();
            else return; // this method is already being run in another async "thread"
        }
        private async void serviceBeatmapChangeRequest()
        {
            // acquire mutually exclusive entry into this method
            serviceBeatmapRequestLocked = true;

            Beatmap candidateOriginalBeatmap = null, candidateNewBeatmap = null;
            while (completedBeatmapRequest == null || completedBeatmapRequest.RequestNumber != mapChangeRequests.Last().RequestNumber)
            {
                completedBeatmapRequest = mapChangeRequests.Last();
                candidateOriginalBeatmap = await Task.Run(() => LoadBeatmap(mapChangeRequests.Last().Name));

                if (candidateOriginalBeatmap != null)
                {
                    candidateNewBeatmap = candidateOriginalBeatmap.Copy();
                    ModifyBeatmapTiming(candidateOriginalBeatmap, candidateNewBeatmap, BpmMultiplier);

                    // Apply bpm scaled settings
                    if (ScaleAR) candidateNewBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedAR(candidateOriginalBeatmap, BpmMultiplier);
                    //if (ScaleOD) candidateNewBeatmap.OverallDifficulty = DifficultyCalculator.CalculateMultipliedOD(candidateOriginalBeatmap, BpmMultiplier);

                    // Apply locked settings
                    if (HpIsLocked) candidateNewBeatmap.HPDrainRate       = lockedHP;
                    if (CsIsLocked) candidateNewBeatmap.CircleSize        = lockedCS;
                    if (ArIsLocked) candidateNewBeatmap.ApproachRate      = lockedAR;
                    if (OdIsLocked) candidateNewBeatmap.OverallDifficulty = lockedOD;
                }

                // if a new request came in, invalidate candidate beatmap and service the new request
            }
            
            // no new requests, we can commit to using this beatmap
            OriginalBeatmap = candidateOriginalBeatmap;
            NewBeatmap      = candidateNewBeatmap;
            if (OriginalBeatmap == null)
            {
                setState(EditorState.NOT_READY);
                NotReadyReason = BadBeatmapReason.ERROR_LOADING_BEATMAP;
                BeatmapSwitched?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if (OriginalBeatmap.Mode != GameMode.osu)
                {
                    setState(EditorState.NOT_READY);
                    NotReadyReason = BadBeatmapReason.DIFF_NOT_OSUSTD;
                    BeatmapSwitched?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    setState(EditorState.READY);
                    RequestDiffCalc();
                    BeatmapSwitched?.Invoke(this, EventArgs.Empty);
                    BeatmapModified?.Invoke(this, EventArgs.Empty);

                }
            }
            serviceBeatmapRequestLocked = false;
        }

        public void RequestDiffCalc()
        {
            diffCalcRequests.Add(new ConcurrentRequest());

            // acquire mutually exclusive entry into this method
            if (!serviceDiffCalcRequestLocked)
                serviceDiffCalcRequest();
            else return; // this method is already being run in another async "thread"
        }
        private async void serviceDiffCalcRequest()
        {
            // acquire mutually exclusive entry into this method
            serviceDiffCalcRequestLocked = true;

            float stars = 0.0f, aim = 0.0f, speed = 0.0f;
            while (completedDiffCalcRequest == null || completedDiffCalcRequest.RequestNumber != diffCalcRequests.Last().RequestNumber)
            {
                completedDiffCalcRequest = diffCalcRequests.Last();
                try
                {
                    (stars, aim, speed) = await Task.Run(() => DifficultyCalculator.CalculateStarRating(NewBeatmap));
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("lol asdfasdf;lkjasdf");
                }
                // if a new request came in, invalidate the diffcalc result and service the new request
            }
            // we serviced the last request, so we can commit to the diffcalc result
            StarRating  = stars;
            AimRating   = aim;
            SpeedRating = speed;
            BeatmapModified?.Invoke(this, EventArgs.Empty);

            serviceDiffCalcRequestLocked = false;
        }

        private void setState(EditorState s)
        {
            State = s;
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetHP(float value)
        {
            if (State != EditorState.READY)
                return;

            NewBeatmap.HPDrainRate = value;
            lockedHP = value;
            BeatmapModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetCS(float value)
        {
            if (State != EditorState.READY)
                return;

            NewBeatmap.CircleSize = value;
            lockedCS = value;
            RequestDiffCalc();
            BeatmapModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetAR(float value)
        {
            if (State != EditorState.READY)
                return;

            NewBeatmap.ApproachRate = value;
            if (ArIsLocked)
                lockedAR = value;

            ScaleAR = false;
            BeatmapModified?.Invoke(this, EventArgs.Empty);
            ControlsModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetARLock(bool locked)
        {
            ArIsLocked = locked;
            if (ArIsLocked)
                ScaleAR = false;
            else
                SetScaleAR(true);
            ControlsModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetScaleAR(bool value)
        {
            ScaleAR = value;

            if (State == EditorState.NOT_READY)
                return;

            if (ScaleAR)
            {
                NewBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedAR(OriginalBeatmap, BpmMultiplier);
                BeatmapModified?.Invoke(this, EventArgs.Empty);
            }
            ArIsLocked = false;
            ControlsModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetOD(float value)
        {
            if (State != EditorState.READY)
                return;

            NewBeatmap.OverallDifficulty = value;
            if (OdIsLocked)
                lockedOD = value;
            BeatmapModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetHPLock(bool locked)
        {
            HpIsLocked = locked;
            if (!locked)
            {
                NewBeatmap.HPDrainRate = OriginalBeatmap.HPDrainRate;
                BeatmapModified?.Invoke(this, EventArgs.Empty);
            }
            ControlsModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetCSLock(bool locked)
        {
            CsIsLocked = locked;
            if (!locked)
            {
                NewBeatmap.CircleSize = OriginalBeatmap.CircleSize;
                BeatmapModified?.Invoke(this, EventArgs.Empty);
            }
            ControlsModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetODLock(bool locked)
        {
            OdIsLocked = locked;
            if (!locked)
            {
                NewBeatmap.OverallDifficulty = OriginalBeatmap.OverallDifficulty;
                BeatmapModified?.Invoke(this, EventArgs.Empty);
            }
            ControlsModified?.Invoke(this, EventArgs.Empty);
        }
        public void SetBpmMultiplier(float multiplier)
        {
            BpmMultiplier = multiplier;

            // make no changes
            if (State == EditorState.NOT_READY)
                return;

            // scale AR
            if (ScaleAR && !ArIsLocked)
                NewBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedAR(OriginalBeatmap, BpmMultiplier);

            // scale OD
            //if (scaleOD && !odIsLocked)
            //    newBeatmap.ApproachRate = DifficultyCalculator.CalculateMultipliedOD(originalBeatmap, bpmMultiplier);
            
            // modify beatmap timing
            ModifyBeatmapTiming(OriginalBeatmap, NewBeatmap, BpmMultiplier);

            RequestDiffCalc();
            BeatmapModified?.Invoke(this, EventArgs.Empty);
        }

        public GameMode? GetMode()
        {
            return OriginalBeatmap.Mode;
        }


        public float GetScaledAR()
        {
            return DifficultyCalculator.CalculateMultipliedAR(OriginalBeatmap, BpmMultiplier);
        }

        public bool NewMapIsDifferent()
        {
            return (
                NewBeatmap.HPDrainRate != OriginalBeatmap.HPDrainRate ||
                NewBeatmap.CircleSize != OriginalBeatmap.CircleSize ||
                NewBeatmap.ApproachRate != OriginalBeatmap.ApproachRate ||
                NewBeatmap.OverallDifficulty != OriginalBeatmap.OverallDifficulty ||
                BpmMultiplier != 1.0f
            );
        }


        // return the new beatmap object if success
        // return null on failure
        private Beatmap LoadBeatmap(string beatmapPath)
        {
            // test if the beatmap is valid before committing to using it
            Beatmap retMap;
            try
            {
                retMap = new Beatmap(beatmapPath);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Bad .osu file format");
                OriginalBeatmap = null;
                NewBeatmap = null;
                return null;
            }
            // Check if beatmap was loaded successfully
            if (retMap.Filename == null && retMap.Title == null)
            {
                Console.WriteLine("Bad .osu file format");
                return null;
            }

            // Check if this map was generated by osu-trainer
            if (retMap.Tags.Contains("osutrainer"))
            {
                string[] diffFiles = Directory.GetFiles(Path.GetDirectoryName(retMap.Filename), "*.osu");
                int candidateSimilarity = int.MaxValue;
                Beatmap candidate = null;
                foreach (string diff in diffFiles)
                {
                    Beatmap map = new Beatmap(diff);
                    if (map.Tags.Contains("osutrainer"))
                        continue;
                    // lower value => more similar
                    int similarity = JunUtils.LevenshteinDistance(retMap.Version, map.Version);
                    if (similarity < candidateSimilarity)
                    {
                        candidate = map;
                        candidateSimilarity = similarity;
                    }
                }
                // just assume this shit is the original beatmap
                if (candidate != null)
                    retMap = candidate;
            }

            return retMap;
        }


        // it is safe to call this function repeatedly
        private void ModifyBeatmapTiming(Beatmap oldMap, Beatmap newMap, float multiplier)
        {
            // Want to divide timestamps since higher multiplier => shorter time
            // OUT: tp.BpmDelay          for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            for (int i = 0; i < oldMap.TimingPoints.Count; i++)
            {
                var originalTimingPoint = oldMap.TimingPoints[i];
                var newTimingPoint = newMap.TimingPoints[i];
                if (originalTimingPoint.InheritsBPM == false)
                {
                    float oldBpm = 60000 / originalTimingPoint.BpmDelay;
                    float newBpm = oldBpm * multiplier;
                    float newDelay = 60000 / newBpm;
                    newTimingPoint.BpmDelay = newDelay;
                    newTimingPoint.Time = (int)(originalTimingPoint.Time / multiplier);
                }
                else
                {
                    newTimingPoint.Time = (int)(originalTimingPoint.Time / multiplier);
                }
            }

            // OUT: event.StartTime      for each event in beatmap
            // OUT: event.EndTime        for each break event in beatmap
            for (int i = 0; i < oldMap.Events.Count; i++)
            {
                var originalEvent = oldMap.Events[i];
                var newEvent = newMap.Events[i];
                newEvent.StartTime = (int)(originalEvent.StartTime / multiplier);
                if (originalEvent.GetType() == typeof(BreakEvent))
                    ((BreakEvent)newEvent).EndTime = (int)(((BreakEvent)originalEvent).EndTime / multiplier);
            }

            // OUT: hitobject.StartTime         for each hit object in beatmap
            // OUT: hitobject.EndTime           for each spinner in beatmap
            for (int i = 0; i < oldMap.HitObjects.Count; i++)
            {
                var originalObject = oldMap.HitObjects[i];
                var newObject = newMap.HitObjects[i];
                newObject.StartTime = (int)(originalObject.StartTime / multiplier);
                if (originalObject.GetType() == typeof(SpinnerObject))
                    ((SpinnerObject)newObject).EndTime = (int)(((SpinnerObject)originalObject).EndTime / multiplier);
            }
        }

        // OUT: beatmap.Version
        // OUT: beatmap.Filename
        // OUT: beatmap.AudioFilename (if multiplier is not 1x)
        // OUT: beatmap.Tags
        private void ModifyBeatmapMetadata(Beatmap map, float multiplier)
        {
            if (multiplier == 1)
            {
                string ARODCS = "";
                if (NewBeatmap.ApproachRate != OriginalBeatmap.ApproachRate)
                    ARODCS += $" AR{NewBeatmap.ApproachRate}";
                if (NewBeatmap.OverallDifficulty != OriginalBeatmap.OverallDifficulty)
                    ARODCS += $" OD{NewBeatmap.OverallDifficulty}";
                if (NewBeatmap.CircleSize != OriginalBeatmap.CircleSize)
                    ARODCS += $" CS{NewBeatmap.CircleSize}";
                map.Version += ARODCS;
            }
            else
            {
                // If song has changed, no ARODCS in diff name
                var bpmsUnique = GetBpmList(map).Distinct().ToList();
                if (bpmsUnique.Count >= 2)
                    map.Version += $" x{multiplier}";
                else
                    map.Version += $" {(bpmsUnique[0]).ToString("0")}bpm";
                map.AudioFilename = map.AudioFilename.Substring(0, map.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture)) + " " + GetBpm(map).Item1 + "bpm.mp3";
            }

            map.Filename = map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + JunUtils.NormalizeText(map.Artist) + " - " + JunUtils.NormalizeText(map.Title) + " (" + JunUtils.NormalizeText(map.Creator) + ")" + " [" + JunUtils.NormalizeText(map.Version) + "].osu";
            // make this map searchable in the in-game menus
            map.Tags.Add("osutrainer");
        }

        // dominant, min, max
        public (float, float, float) GetOriginalBpmData() => GetBpm(OriginalBeatmap);
        // dominant, min, max
        public (float, float, float) GetNewBpmData() => GetBpm(NewBeatmap);
        private (float, float, float) GetBpm(Beatmap map)
        {
            var bpmList = GetBpmList(map).Select((bpm) => (int)bpm).ToList();
            if (bpmList.Count == 0)
            {
                Console.WriteLine("Very bad.");
            }
            bpmList = bpmList.Distinct().ToList();

            if (bpmList.Count == 1)
                return (bpmList[0], bpmList[0], bpmList[0]);

            return (GetDominantBpm(map), bpmList.Min(), bpmList.Max());

            float GetDominantBpm(Beatmap m)
            {
                var bpms = GetBpmList(m);
                if (bpms.Count == 1)
                    return bpms[0];
                // store bpm => prominence (as in, how long that bpm is active in the map)
                float previousTime = 0;
                float previousBpm = 0;
                var bpmTimingPoints = m.TimingPoints.Where(tp => !tp.InheritsBPM).ToList();
                var bpmProminenceValues = new Dictionary<float, float>();

                for (int i = 0; i < bpmTimingPoints.Count; i++)
                {
                    var tp = bpmTimingPoints[i];
                    var currentBpm = 60000 / tp.BpmDelay;
                    var currentTime = tp.Time;
                    // case: first timing point
                    if (i == 0)
                    {
                        previousBpm = currentBpm;
                        previousTime = currentTime;
                    }
                    // case: middle timing point
                    else if (i < bpmTimingPoints.Count - 1)
                    {
                        if (!bpmProminenceValues.ContainsKey(previousBpm))
                            bpmProminenceValues.Add(previousBpm, 0);
                        float duration = currentTime - previousTime;
                        bpmProminenceValues[previousBpm] += duration;

                        previousBpm = currentBpm;
                        previousTime = currentTime;
                    }
                    // case: last timing point
                    else if (i == bpmTimingPoints.Count - 1)
                    {
                        if (!bpmProminenceValues.ContainsKey(previousBpm))
                            bpmProminenceValues.Add(previousBpm, 0);
                        float duration = currentTime - previousTime;
                        bpmProminenceValues[previousBpm] += duration;

                        // jump ahead in time to last hit object in map
                        if (!bpmProminenceValues.ContainsKey(currentBpm))
                            bpmProminenceValues.Add(currentBpm, 0);
                        float finalTime = m.HitObjects.Last().StartTime;
                        bpmProminenceValues[currentBpm] += finalTime - currentTime;
                    }
                }
                var lines = bpmProminenceValues.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
                Console.WriteLine(string.Join(Environment.NewLine, lines));

                float candidateBpm = 0;
                float maxProminence = float.MinValue;
                foreach (KeyValuePair<float, float> entry in bpmProminenceValues)
                {
                    if (entry.Value > maxProminence)
                    {
                        candidateBpm = entry.Key;
                        maxProminence = entry.Value;
                    }
                }
                return candidateBpm;
            }
        }

        private List<float> GetBpmList(Beatmap map)
        {
            if (map == null || map.TimingPoints.Count == 0)
                return new List<float> { 0.0f };

            // special case
            if (map.TimingPoints.Count == 1)
                return new List<float> { 60000 / map.TimingPoints[0].BpmDelay };

            var bpms = map.TimingPoints.Where((tp) => !tp.InheritsBPM).Select((tp) => 60000 / tp.BpmDelay).ToList();
            var bpmsUnique = bpms.Distinct().ToList();
            if (bpmsUnique.Count == 1)
                return bpmsUnique;
            return bpms;
        }

        public void ResetBeatmap()
        {
            NewBeatmap.HPDrainRate = OriginalBeatmap.HPDrainRate;
            NewBeatmap.CircleSize = OriginalBeatmap.CircleSize;
            NewBeatmap.ApproachRate = OriginalBeatmap.ApproachRate;
            NewBeatmap.OverallDifficulty = OriginalBeatmap.OverallDifficulty;
            HpIsLocked = false;
            CsIsLocked = false;
            ArIsLocked = false;
            OdIsLocked = false;
            ScaleAR = true;
            ScaleOD = true;
            BpmMultiplier = 1.0f;
            ModifyBeatmapTiming(OriginalBeatmap, NewBeatmap, 1.0f);
            RequestDiffCalc();
            ControlsModified?.Invoke(this, EventArgs.Empty);
            BeatmapModified?.Invoke(this, EventArgs.Empty);
        }
    }
}
