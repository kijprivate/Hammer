using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{

    public delegate void Event();
    public delegate void Event<A>(A arg1);

    // Two examples how to add new events:
    //
    //public static Event NewEvent;
    //public static void RaiseEventNewEvent()
    //{
    //    if (NewEvent != null) NewEvent();
    //}
    //
    //public static Event<int, bool> NewEvent;
    //public static void RaiseEventNewEvent(int howMany, bool isFinal)
    //{
    //    if (NewEvent != null) NewEvent(howMany, isFinal);
    //}

    public static Event EventMenuHided;
    public static void RaiseEventMenuHided()
    {
        if (EventMenuHided != null) EventMenuHided();
    }

    public static Event EventGameStarted;
    public static void RaiseEventGameStarted()
    {
        if (EventGameStarted != null) EventGameStarted();
    }

    public static Event EventGameOver;
    public static void RaiseEventGameOver()
    {
        if (EventGameOver != null) EventGameOver();
    }

    public static Event EventGameResumed;
    public static void RaiseEventGameResumed()
    {
        if (EventGameResumed != null) EventGameResumed();
    }

    public static Event EventHammerHit;
    public static void RaiseEventHammerHit()
    {
        if (EventHammerHit != null) EventHammerHit();
    }

    public static Event EventNailPocket;
    public static void RaiseEventNailPocket()
    {
        if (EventNailPocket != null) EventNailPocket();
    }
    
    public static Event EventNailFinished;
    public static void RaiseEventNailFinished()
    {
        if (EventNailFinished != null) EventNailFinished();
    }
    
    public static Event EventPerfectHit;
    public static void RaiseEventPerfectHit()
    {
        if (EventPerfectHit != null) EventPerfectHit();
    }

    public static Event EventNoMoreNails;
    public static void RaiseEventNoMoreNails()
    {
        if (EventNoMoreNails != null) EventNoMoreNails();
    }

    public static Event EventCoinsSubstracted;
    public static void RaiseEventCoinsSubstracted()
    {
        if (EventCoinsSubstracted != null) EventCoinsSubstracted();
    }

    public static Event EventHammerSpriteChanged;
    public static void RaiseEventHammerSpriteChanged()
    {
        if (EventHammerSpriteChanged != null) EventHammerSpriteChanged();
    }

    public static Event<int> EventShowSplash;   // -1 - not enough strength 0 - perfect 1 - too hard
    public static void RaiseEventShowSplash(int splashId)
    {
        if (EventShowSplash != null) EventShowSplash(splashId);
    }

    public static Event<int> EventEarnScore;
    public static void RaiseEventEarnScore(int points)
    {
        if (EventEarnScore != null) EventEarnScore(points);
    }
}
