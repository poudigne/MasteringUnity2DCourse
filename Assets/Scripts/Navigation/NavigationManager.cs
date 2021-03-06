﻿using System;
using System.Collections.Generic;
using UnityEngine;

public static class NavigationManager
{

  public static string previousLocation;


  public enum Worlds
  {
    // places
    Battle,
    World,
    Shop,
    // Cities
    Cave,
    Home,
    Kirkidw
  }

  public struct Route
  {
    public string routeDescription;
    public bool canTravel;
  }

  public static Dictionary<Worlds, Route> routeInformation = new Dictionary<Worlds, Route>()
  {
    {Worlds.World, new Route {routeDescription = "The big bad world", canTravel = true} },
    {Worlds.Cave, new Route {routeDescription = "The deep dark cave", canTravel = false} },
    {Worlds.Home, new Route {routeDescription = "Home sweet home!", canTravel = true} },
    {Worlds.Kirkidw, new Route {routeDescription = "The grand city of Kirkidw", canTravel = true} },
    {Worlds.Shop, new Route { canTravel = true} },
  };

  public static string GetRouteInfo(Worlds destination)
  {
    return routeInformation.ContainsKey(destination) ? routeInformation[destination].routeDescription : null;
  }
  public static string GetRouteInfo(string destination)
  {
    var worldEnum = (Worlds)Enum.Parse(typeof(Worlds), destination);
    return GetRouteInfo(worldEnum);
  }

  public static bool CanNavigate(Worlds destination)
  {
    return routeInformation.ContainsKey(destination) && routeInformation[destination].canTravel;
  }
  public static bool CanNavigate(string destination)
  {
    var worldEnum = (Worlds)Enum.Parse(typeof (Worlds), destination) ;
    return CanNavigate(worldEnum);
  }

  public static void NavigateTo(Worlds destination)
  {
    previousLocation = Application.loadedLevelName;
    if (destination == Worlds.Home)
      GameState.playerReturningHome = false;
    FadeinOutManager.FadeToLevel(destination.ToString(), 2.0f, 2.0f, Color.black);
  }
  public static void NavigateTo(string destination)
  {
    var worldEnum = (Worlds)Enum.Parse(typeof(Worlds), destination);
    NavigateTo(worldEnum);
  }

  public static void GoBack()
  {
    var backLocation = previousLocation;
    previousLocation = Application.loadedLevelName;
    NavigateTo(backLocation);
  }
}
