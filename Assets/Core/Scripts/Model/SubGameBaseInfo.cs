using System.Collections.Generic;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;

namespace UniKid.Core.Model
{
    public sealed class SubGameBaseInfo
    {
        public SubGameName Name { get; set; }
        public bool IsEnabled { get; set; }
        public SubGameCategory Category { get; set; }

        public List<LevelBaseInfo> LevelList { get; set; }

        public SubGameBaseInfo()
        {
            LevelList = new List<LevelBaseInfo>();
        }
    }


    public class LevelBaseInfo : Level<Stage>
    {
        private Level<Stage> _levelSettings;

        public int Id { get; set; }
        public SubGameName SubGameName { get; set; }
        public bool IsPassed { get; set; }
        public int AttemptNum { get; set; }

        public string UniqueId
        {
            get { return string.Format("{0}_{1}", SubGameName, Id); }
        }

        public Level<Stage> LevelSettings
        {
            get
            {
                if (_levelSettings != null) return _levelSettings;

                _levelSettings = new Level<Stage> { Name = Name, StageArray = StageArray, TagSequence = TagSequence };

                return _levelSettings;
            }
        }
    }
}