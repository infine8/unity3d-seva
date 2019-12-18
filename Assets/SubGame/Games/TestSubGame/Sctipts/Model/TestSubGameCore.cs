using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;
using UnityEngine;

namespace UniKid.SubGame.Games.TestSubGame.Model
{
    public sealed class TestSubGameCore : SubGameCoreBase<TestSubGameSettings, TestSubGameUserData>
    {
        public override SubGameName SubGameName
        {
            get { return SubGameName.TestSubGame; }
        }

        public TestSubGameCore()
        {
            
        }

    }
}