﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

public class MessagingManager : Singleton<MessagingManager>
{
  protected MessagingManager() { } // guarantee this will be always a singleton only - can't use the constructor!

  // public property for manager
  private List<Action> subscribers = new List<Action>();
  private List<Action<bool>> uiEventSubscribers = new List<Action<bool>>();

  // Subscribe method for manager
  public void Subscribe(Action subscriber)
  {
    if (subscribers != null)
    {
      subscribers.Add(subscriber);
    }
  }

  // Unsubscribe method for manager
  public void UnSubscribe(Action subscriber)
  {
    if (subscribers != null)
    {
      subscribers.Remove(subscriber);
    }
  }

  // Clear subscribers method for manager
  public void ClearAllSubscribers()
  {
    if (subscribers != null)
    {
      subscribers.Clear();
    }
  }

  public void Broadcast()
  {
    if (subscribers != null)
    {
      foreach (var subscriber in subscribers.ToArray())
      {
        subscriber();
      }
    }
  }

  // Subscribe method for manager
  public void SubscribeUIEvent(Action<bool> subscriber)
  {
    if (uiEventSubscribers != null)
    {
      uiEventSubscribers.Add(subscriber);
    }
  }

  public void BroadcastUIEvent(bool uiVisible)
  {
    if (uiEventSubscribers != null)
    {
      foreach (var subscriber in uiEventSubscribers.ToArray())
      {
        subscriber(uiVisible);
      }
    }
  }

  // Unsubscribe method for UI manager
  public void UnSubscribeUIEvent(Action<bool> subscriber)
  {
    if (uiEventSubscribers != null)
    {
      uiEventSubscribers.Remove(subscriber);
    }
  }

  // Clear subscribers method for manager
  public void ClearAllUIEventSubscribers()
  {
    if (uiEventSubscribers != null)
    {
      uiEventSubscribers.Clear();
    }
  }
}


