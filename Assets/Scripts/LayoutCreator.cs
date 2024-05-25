using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LayoutCreator : MonoBehaviour {
    bool _isLayoutMode;
    [SerializeField]List<Vector3Int> _selectedPositions;
    [SerializeField] Tilemap boardTilemap;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Button saveToJsonButton;
    LayoutList _layoutList;
    
    void Awake() {
        _selectedPositions = new List<Vector3Int>( 46 );
        _layoutList = new LayoutList();
    }

    void Update() {
        if ( !_isLayoutMode ) return;
        //Get mouse position on left click and try to add selected position
        if ( Input.GetMouseButtonDown( 0 ) ) {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            mouseWorldPos.z = 0;
            var tilePos = boardTilemap.WorldToCell( mouseWorldPos );
            Debug.Log($"MOUSE 0 CLICKED AT {tilePos}");
            SelectPosition( tilePos );
        }
        
        //Color tiles in tilemap red if the tiles position exist in the list
        foreach ( var position in _selectedPositions ) {
            boardTilemap.SetTileFlags( position, TileFlags.None );
            boardTilemap.SetColor( position, Color.red );
        }
    }

    public void ToggleLayoutMode() {
        if ( _isLayoutMode ) {
            //Set all tiles in tilemap to the color white
            foreach ( var position in _selectedPositions ) {
                boardTilemap.SetTileFlags( position, TileFlags.None );
                boardTilemap.SetColor( position, Color.white );
            }
            _isLayoutMode = false;
            _selectedPositions.Clear();
            buttonText.text = "Start Layout Mode";
            saveToJsonButton.interactable = false;
            
            return;
        }
        _isLayoutMode = true;
        buttonText.text = "End Layout Mode";
        saveToJsonButton.interactable = true;
    }

    public void SelectPosition( Vector3Int position ) {
        //If the position doesn't have a tile in the tilemap then return
        Debug.Log(boardTilemap.GetTile( position ) is not Tile);
        if ( boardTilemap.GetTile( position ) is not Tile ) return;
        //If position is already in list then remove it otherwise add it.
        if ( _selectedPositions.Contains( position ) ) {
            _selectedPositions.Remove( position );
        }
        else {
            _selectedPositions.Add( position );
        }
    }

    public void SaveToJson() {
        if ( !_isLayoutMode ) return;
        _layoutList.positions = _selectedPositions;
        var json = JsonUtility.ToJson( _layoutList );
        System.IO.File.WriteAllText( "Assets/Resources/layout2.json", json );
    }
}

[Serializable]
public class LayoutList {
    public List<Vector3Int> positions = new();
}