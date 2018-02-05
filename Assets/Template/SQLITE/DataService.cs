﻿using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{

    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    public int  CreateGameplay()
    {

        //TODO: Linking it do the expermental parameters we have 
        Debug.Log("streetsDirections " + ExperementParameters.streetsDirections);
        Debug.Log("numberOfPathsPerStreet " + ExperementParameters.numberOfPathsPerStreet);
        Debug.Log("carsSpeed " + ExperementParameters.carsSpeed);
        Debug.Log("distanceBetweenCars   " + ExperementParameters.distanceBetweenCars);
        Debug.Log("soundDirections " + ExperementParameters.soundDirections);
        Debug.Log("lengthOfPatient "+ ExperementParameters.lengthOfPatient);
        Gameplay gamePlay = new Gameplay
        {
            street_direction = ExperementParameters.streetsDirections,
            pathes_per_direction = ExperementParameters.numberOfPathsPerStreet,
            car_speed_km = ExperementParameters.carsSpeed,
            car_span_km = ExperementParameters.distanceBetweenCars,
            sound_mode = ExperementParameters.soundDirections,
            player_name = "Omar",
            player_height = (double)ExperementParameters.lengthOfPatient
        };

        Debug.Log("CREATED GAMEPLAY TO SQLITE DB");
       return _connection.Insert(gamePlay);
    }

    public void CreateRoadCrossingData(ObservedData observedData)
                         
    {
        
        
        StreetCrossingData streetCrossingData;
        
        for (int i = 0; i < observedData.traffic_towards_flow.Count; i++)
        {
        streetCrossingData = new StreetCrossingData
        {
            gameplay_id = ExperementParameters.gameplay_id,  //storing from the static variable in the class
            traffic_flow_towards = observedData.traffic_towards_flow[i],
            current_time_span = observedData.current_time_span[i],
            current_distance_nearest_car = 0,
            gazing_car = observedData.isLookingAtCar[i],
            gazing_nearest_car = false,
            after_collision_frame = observedData.is_hit_by_car[i],

            person_x = (double) observedData.playerPositions[i].x,
            person_y = (double) observedData.playerPositions[i].y,
            person_z =(double) observedData.playerPositions[i].z,

            head_rotation= (double) observedData.playerHeadRotations[i]
        };
        _connection.Insert(streetCrossingData);
        Debug.Log("CREATED SCD TO SQLITE DB");
        }
    }

    public Gameplay GetGameplayByID(int gameplay_id)  //stored in playerPref 
    {

        return _connection.Table<Gameplay>().Where(x => x.gameplay_id == gameplay_id).First();

    }
    public int GetGameplayIDFromDatabase()  //stored in playerPref 
    {
        Debug.Log("GET  GAMEPLAY ID FROM SQLITE DB");
        return _connection.Table<Gameplay>().Select(x => x.gameplay_id).Count();
        //return  _connection.Table<Gameplay>().Where(x => x.gameplay_id == gameplay_id_last).First();

    }

    /*******************************************return table in array form so we can extract it using foreach************************************/
    public IEnumerable<Gameplay> GetGameplayTable()
    {
        return _connection.Table<Gameplay>();
    }
    public IEnumerable<StreetCrossingData> GetRoadCrossingDataTable()
    {
        return _connection.Table<StreetCrossingData>();
    }
    public IEnumerable<GrabbedObjects> GetGrabbedObjectDataTable()
    {
        return _connection.Table<GrabbedObjects>();
    }

    /***********************************************GET DATA FROM DB******************************************** */

}
