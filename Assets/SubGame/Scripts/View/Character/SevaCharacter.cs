namespace UniKid.SubGame.View.Character
{
    public class SevaCharacter : CharacterBase
    {
        public static readonly string ANIMATION_FILE_NAME = "seva_all";


        protected override string AnimationFileName
        {
            get { return ANIMATION_FILE_NAME; }
        }
    }
}