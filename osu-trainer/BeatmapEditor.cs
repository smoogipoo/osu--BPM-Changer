using BMAPI.v1;
using BMAPI.v1.Events;
using BMAPI.v1.HitObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

    class BeatmapEditor
    {
        MainForm form;
        public BadBeatmapReason NotReadyReason;

        public Beatmap OriginalBeatmap;
        public Beatmap NewBeatmap;

        object recalcLock = new object();
        bool recalcNeeded = false;
        public float starRating;
        public float aimRating;
        public float speedRating;
        float lockedHP = 0f;
        float lockedCS = 0f;
        float lockedAR = 0f;
        float lockedOD = 0f;

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
            ModifyBeatmapTiming(BpmMultiplier);
            ModifyBeatmapMetadata(NewBeatmap, BpmMultiplier);
            if (!File.Exists(Helper.GetBeatmapDirectoryName(OriginalBeatmap) + "\\" + NewBeatmap.AudioFilename))
                await Task.Run(() => SongSpeedChanger.GenerateAudioFile(OriginalBeatmap, NewBeatmap, BpmMultiplier));
            NewBeatmap.Save();

            // post
            form.PlayDoneSound();

            // reset diff name
            NewBeatmap.Version = OriginalBeatmap.Version;

            setState(EditorState.READY);
        }


        // TODO: simulate long load time and test
        // TODO: set update interval to be really large and test
        public async void RequestBeatmapLoad(string beatmapPath)
        {
            bool success = await Task.Run(() => LoadBeatmap(beatmapPath));
            if (!success)
            {
                setState(EditorState.NOT_READY);
                NotReadyReason = BadBeatmapReason.ERROR_LOADING_BEATMAP;
                BeatmapSwitched?.Invoke(this, EventArgs.Empty);
                return; // get out of here
            }

            // finish
            ModifyBeatmapTiming(BpmMultiplier); // for calculating star rating
            setState(EditorState.READY);
            BeatmapSwitched?.Invoke(this, EventArgs.Empty);
            BeatmapModified?.Invoke(this, EventArgs.Empty);
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
            ModifyBeatmapTiming(BpmMultiplier);

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

        public async void RecalculateStarRating()
        {
            if (State != EditorState.READY)
                return;
            if (!recalcNeeded)
                return;

            // try to get exclusive access
            if (Monitor.TryEnter(recalcLock))
            {
                recalcNeeded = false;

                // BEGIN: time consuming section
                float stars, aimStars, speedStars = -1.0f;
                try
                {
                    (stars, aimStars, speedStars) = await Task.Run(() => DifficultyCalculator.CalculateStarRating(NewBeatmap));
                }
                catch (NullReferenceException e)
                {
                    // just do nothing, wait for next chance to recalculate difficulty
                    Console.WriteLine(e);
                    Console.WriteLine("lol asdfasdf;lkjasdf");
                    return;
                }
                if (stars < 0)
                    return;

                int aimPercent = (int)(100.0f * aimStars / (aimStars + speedStars));
                int speedPercent = 100 - aimPercent;
                BeatmapModified?.Invoke(this, EventArgs.Empty);
                // END: time consuming section

                // release lock
                Monitor.Exit(recalcLock);
            }
        }

        #region private

        // return true on success
        private bool LoadBeatmap(string beatmapPath)
        {
            // test if the beatmap is valid before committing to using it
            Beatmap test;
            try
            {
                test = new Beatmap(beatmapPath);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Bad .osu file format");
                OriginalBeatmap = null;
                NewBeatmap = null;
                return false;
            }
            // Check if beatmap was loaded successfully
            if (test.Filename == null && test.Title == null)
            {
                Console.WriteLine("Bad .osu file format");
                return false;
            }

            // Check if this map was generated by osu-trainer
            if (test.Tags.Contains("osutrainer"))
            {
                string[] diffFiles = Directory.GetFiles(Path.GetDirectoryName(test.Filename), "*.osu");
                int candidateSimilarity = int.MaxValue;
                Beatmap candidate = null;
                foreach (string diff in diffFiles)
                {
                    Beatmap map = new Beatmap(diff);
                    if (map.Tags.Contains("osutrainer"))
                        continue;
                    // lower value => more similar
                    int similarity = Helper.LevenshteinDistance(test.Version, map.Version);
                    if (similarity < candidateSimilarity)
                    {
                        candidate = map;
                        candidateSimilarity = similarity;
                    }
                }
                // just assume this shit is the original beatmap
                if (candidate != null)
                    test = candidate;
            }

            // Commit to new beatmap
            OriginalBeatmap = new Beatmap(test.Filename);
            NewBeatmap = new Beatmap(test.Filename);

            // Apply locked settings
            if (HpIsLocked) NewBeatmap.HPDrainRate       = lockedHP;
            if (CsIsLocked) NewBeatmap.CircleSize        = lockedCS;
            if (ArIsLocked) NewBeatmap.ApproachRate      = lockedAR;
            if (OdIsLocked) NewBeatmap.OverallDifficulty = lockedOD;
            return true;
        }


        // it is safe to call this function repeatedly
        private void ModifyBeatmapTiming(float multiplier)
        {
            // Want to divide timestamps since high multiplier => shorter time
            // OUT: tp.BpmDelay          for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            // OUT: tp.Time              for each timing point in beatmap
            for (int i = 0; i < OriginalBeatmap.TimingPoints.Count; i++)
            {
                var originalTimingPoint = OriginalBeatmap.TimingPoints[i];
                var newTimingPoint = NewBeatmap.TimingPoints[i];
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
            for (int i = 0; i < OriginalBeatmap.Events.Count; i++)
            {
                var originalEvent = OriginalBeatmap.Events[i];
                var newEvent = NewBeatmap.Events[i];
                newEvent.StartTime = (int)(originalEvent.StartTime / multiplier);
                if (originalEvent.GetType() == typeof(BreakEvent))
                    ((BreakEvent)newEvent).EndTime = (int)(((BreakEvent)originalEvent).EndTime / multiplier);
            }

            // OUT: hitobject.StartTime         for each hit object in beatmap
            // OUT: hitobject.EndTime           for each spinner in beatmap
            for (int i = 0; i < OriginalBeatmap.HitObjects.Count; i++)
            {
                var originalObject = OriginalBeatmap.HitObjects[i];
                var newObject = NewBeatmap.HitObjects[i];
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
                map.AudioFilename = map.AudioFilename.Substring(0, map.AudioFilename.LastIndexOf(".", StringComparison.InvariantCulture)) + Helper.NormalizeText(map.Version) + ".mp3";
            }

            map.Filename = map.Filename.Substring(0, map.Filename.LastIndexOf("\\", StringComparison.InvariantCulture) + 1) + Helper.NormalizeText(map.Artist) + " - " + Helper.NormalizeText(map.Title) + " (" + Helper.NormalizeText(map.Creator) + ")" + " [" + Helper.NormalizeText(map.Version) + "].osu";
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
            if (map == null)
                return new List<float> { 0.0f };
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
            ModifyBeatmapTiming(1.0f);
            ControlsModified?.Invoke(this, EventArgs.Empty);
            BeatmapModified?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
