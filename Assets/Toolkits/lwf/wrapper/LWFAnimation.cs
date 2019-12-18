using System;
using LWF;
using UniKid.SubGame.Model;
using UniKid.SubGame.View;
using UnityEngine;


public sealed class LWFAnimation : LWFObject
{
    public static readonly string DEFAULT_LWF_RESOURCE_FOLDER_NAME = "LWF/";

    public Movie Movie { get; private set; }

    public Action OnPlayFinished;
    public Action OnPlayStart;
    public Action OnInit;

    private SubGameCoreBase SubGameCore { get { return MainView.SubGameCoreBase; } }

    private SubGameMainViewBase MainView { get; set; }

    private bool _isMoviePlaying = false;

    private bool _isInited = false;

    protected override void Update()
    {
        base.Update();

        if (MainView != null && SubGameCore != null && _isInited) return;

        _isInited = true;

        if (MainView == null) MainView = (SubGameMainViewBase) FindObjectOfType(typeof (SubGameMainViewBase));
 
        var dir = DEFAULT_LWF_RESOURCE_FOLDER_NAME;
        var movName = string.IsNullOrEmpty(lwfName) ? name : lwfName;

        if (!Load(dir + movName, dir))
        {
            dir = SubGameCore.SubGameName.ToString() + "/";
            Load(dir + movName, dir);
            Resources.UnloadUnusedAssets();
        }

        Movie = lwf.rootMovie;
        
        AddMovieEventHandler(Movie.GetFullName(), enterFrame: OnMovieFinished);

        if (OnInit != null) OnInit();

        transform.SendMessage("LWFAnimationHasBeenInited", this, SendMessageOptions.DontRequireReceiver);
    }

    public void Play(string animationName = null)
    {
        _isMoviePlaying = true;

        if (string.IsNullOrEmpty(animationName))
        {
            Movie.Play();
        }
        else
        {
            Movie.GotoAndPlay(animationName);
        }

        if (OnPlayStart != null) OnPlayStart();
    }

    public void Stop()
    {
        Movie.Stop();
    }

    private void OnMovieFinished(Movie movie)
    {
        if ((movie.currentFrame == movie.totalFrames || !movie.playing) && _isMoviePlaying)
        {
            if (OnPlayFinished != null) OnPlayFinished();

            _isMoviePlaying = false;
        }
    }
}
