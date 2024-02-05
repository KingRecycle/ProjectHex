using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CharlieMadeAThing.ProjectHex {
    public class BoardSetup : MonoBehaviour {

        public Tilemap boardTilemap;
        [SerializeField] Tile validBoardTile;
        Dictionary<Vector3Int, Node> _nodes;
        [SerializeField] OrbTypeData orbPrefabLookup;
        Dictionary<OrbType, int> _currentCounts;
        Dictionary<OrbType, int> _maxCounts;
        [SerializeField]List<OrbType> bagOfOrbs;
    
        //UI
        [SerializeField] GameObject trackerPanel;
        [SerializeField] GameObject trackerUIPrefab;
        [SerializeField] TMP_Text orbCountText;

        readonly Vector3Int[] _directions = {
            new Vector3Int( 0, 1, 0 ), // Even Row - Up Right
            new Vector3Int( 1, 0, 0 ), // Even Row - right
            new Vector3Int( 0, -1, 0 ), // Even Row - Right Down
            new Vector3Int( -1, -1, 0 ), // Even Row - Left Down
            new Vector3Int( -1, 0, 0 ), // Even Row - Left
            new Vector3Int( -1, 1, 0 ), // Even Row - Left Up
            //---------\\
            new Vector3Int( 1, 1, 0 ), // Odd Row - Up Right
            new Vector3Int( 1, 0, 0 ), // Odd Row - Right
            new Vector3Int( 1, -1, 0 ), // Odd Row - Right Down
            new Vector3Int( 0, -1, 0 ), // Odd Row - Left Down
            new Vector3Int( -1, 0, 0 ), // Odd Row - Left
            new Vector3Int( 0, 1, 0 ), // Odd Row - Left Up
            
        };
        
        void InitializeBoard() {
            _nodes = new Dictionary<Vector3Int, Node>();
            //Grab all tiles in Tilemap and created a node for each and add them to dictionary
            foreach ( var position in boardTilemap.cellBounds.allPositionsWithin ) {
                if( _nodes.ContainsKey(position)) continue;
                var localPlace = new Vector3Int( position.x, position.y, 0 );
                
                if ( !boardTilemap.HasTile( localPlace ) ) {
                    continue;
                }
                // Debug.Log( $"Position: {position} ::: Cell Position: {localPlace}" );
                var node = new Node {
                    Position = localPlace,
                    CurrentOrb = OrbType.None,
                    IsClickable = false,
                    Tile = boardTilemap.GetTile<Tile>(localPlace)
                };

                if ( node.Tile != validBoardTile ) {
                    node.CurrentOrb = OrbType.NonPlayable;
                }
                _nodes.Add( localPlace, node );
            }

            foreach ( var kvnode in _nodes ) {
                var node = kvnode.Value;
                if ( node.CurrentOrb is OrbType.NonPlayable ) continue;

                if ( node.Position.y % 2 == 0 ) {
                    //Check up right
                    var topRightNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[0];
                    node.Neighbors.Add( _nodes[topRightNeighborPosition] );

                    //Check right
                    var rightNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[1];
                    node.Neighbors.Add( _nodes[rightNeighborPosition] );

                    //Check bottom right
                    var bottomRightNeighborPosition =
                        new Vector3Int( node.Position.x, node.Position.y ) + _directions[2];
                    node.Neighbors.Add( _nodes[bottomRightNeighborPosition] );

                    //check bottom left
                    var bottomLeftNeighborPosition =
                        new Vector3Int( node.Position.x, node.Position.y ) + _directions[3];
                    node.Neighbors.Add( _nodes[bottomLeftNeighborPosition] );

                    //check left
                    var leftNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[4];
                    node.Neighbors.Add( _nodes[leftNeighborPosition] );

                    //check top left
                    var topLeftNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[5];
                    node.Neighbors.Add( _nodes[topLeftNeighborPosition] );
                }
                else {
                    //Check up right
                    var topRightNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[6];
                    node.Neighbors.Add( _nodes[topRightNeighborPosition] );

                    //Check right
                    var rightNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[7];
                    node.Neighbors.Add( _nodes[rightNeighborPosition] );

                    //Check bottom right
                    var bottomRightNeighborPosition =
                        new Vector3Int( node.Position.x, node.Position.y ) + _directions[8];
                    node.Neighbors.Add( _nodes[bottomRightNeighborPosition] );

                    //check bottom left
                    var bottomLeftNeighborPosition =
                        new Vector3Int( node.Position.x, node.Position.y ) + _directions[9];
                    node.Neighbors.Add( _nodes[bottomLeftNeighborPosition] );

                    //check left
                    var leftNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[10];
                    node.Neighbors.Add( _nodes[leftNeighborPosition] );

                    //check top left
                    var topLeftNeighborPosition = new Vector3Int( node.Position.x, node.Position.y ) + _directions[11];
                    node.Neighbors.Add( _nodes[topLeftNeighborPosition] );
                }
            }

            _currentCounts = new Dictionary<OrbType, int>() {
                { OrbType.None, 0 },
                { OrbType.Fire, 0 },
                { OrbType.Water, 0 },
                { OrbType.Earth, 0 },
                { OrbType.Air, 0 },
                { OrbType.Nuke, 0 },
                { OrbType.Zero, 0 },
                { OrbType.One, 0 },
                { OrbType.Save, 0 },
                { OrbType.NetworkOne, 0 },
                { OrbType.NetworkTwo, 0 },
                { OrbType.NetworkThree, 0 },
                { OrbType.NetworkFour, 0 },
                { OrbType.NetworkFive, 0 },
                { OrbType.Encrypt, 0 },
            };

            bagOfOrbs = new List<OrbType>();
            //Add 8 of each orb type
            for ( var i = 0; i < 8; i++ ) {
                bagOfOrbs.Add( OrbType.Air );
                bagOfOrbs.Add( OrbType.Earth );
                bagOfOrbs.Add( OrbType.Fire );
                bagOfOrbs.Add( OrbType.Water );
            }
        
            for ( var i = 0; i < 4; i++ ) {
                bagOfOrbs.Add( OrbType.Nuke );
            }
        
            for ( var i = 0; i < 4; i++ ) {
                bagOfOrbs.Add( OrbType.One );
            }
        
            for ( var i = 0; i < 4; i++ ) {
                bagOfOrbs.Add( OrbType.Zero );
            }
        
        
            for ( var i = 0; i < 5; i++ ) {
                bagOfOrbs.Add( OrbType.Save );
            }
        
            bagOfOrbs.Add(OrbType.NetworkOne);
            bagOfOrbs.Add(OrbType.NetworkTwo);
            bagOfOrbs.Add(OrbType.NetworkThree);
            bagOfOrbs.Add(OrbType.NetworkFour);
            bagOfOrbs.Add(OrbType.NetworkFive);
        
            //Shuffle bagOfOrbs
            bagOfOrbs.Shuffle();
        
            //Add orb to start of list
            bagOfOrbs.Insert( 0, OrbType.Encrypt );
        
        
        }

        void Start() {
            InitializeBoard();
            SetupBoard();
        }

        void Update() {
            //Count the total amount of orbs
            var totalOrbs = _currentCounts.Values.Sum();
            orbCountText.text = "Orb Count: " + totalOrbs;

            foreach ( var node in _nodes ) {
                node.Value.DoTick();
            }
        }

        void SetupBoard() {
            //Load JSON file "layout.json" from Resources folder
            var json = Resources.Load<TextAsset>( "layout" ).text;
            var layout = JsonUtility.FromJson<LayoutList>( json );
            //Loop through positions in LayoutList and randomly place up to 8 of each OrbType
            StartCoroutine( SpawnOrbs( layout ) );
        
        }

        void PlaceOrb( Vector3Int pos, OrbType type ) {
            if ( !IsPositionEmpty( pos ) ) {
                Debug.LogWarning( $"Position {pos} is not empty or valid!" );
                return;
            }

            if ( !orbPrefabLookup.orbTypeToPrefab.TryGetValue( type, out var prefab ) ) {
                Debug.LogWarning( $"OrbType {type} not found in orbTypeToPrefab dictionary!" );
                return;
            }

            var cellPos = boardTilemap.CellToWorld( pos );
            var newOrb = Instantiate( prefab, cellPos, Quaternion.identity );
            newOrb.transform.SetParent( boardTilemap.transform );
            _nodes[pos].ChangeOrbType(type);
            _currentCounts[type]++;
            _nodes[pos].CurrentOrbGameObject = newOrb;
            _nodes[pos].CurrentOrbSpriteRenderer = newOrb.GetComponentInChildren<SpriteRenderer>();
        }

        public void RemoveOrb( Vector3Int pos ) {
            if ( !IsValidPosition( pos ) ) {
                Debug.LogWarning( $"Position {pos} is not valid!" );
                return;
            }
            _currentCounts[_nodes[pos].CurrentOrb]--;
            _nodes[pos].CurrentOrb = OrbType.None;
            //Destroy(_nodes[pos].CurrentOrbGameObject);
        }

        public Node GetNode( Vector3Int position ) {
            return _nodes.GetValueOrDefault( position );
        }

        bool IsValidPosition( Vector3Int pos ) {
            return _nodes.ContainsKey( pos ) && _nodes[pos].Tile == validBoardTile;
        }
        
        bool IsPositionEmpty( Vector3Int pos ) {
            if ( IsValidPosition( pos ) ) {
                return _nodes[pos].CurrentOrb == OrbType.None;
            }

            Debug.LogWarning( $"Position isn't valid!" );
            return false;
        }
    
        //Coroutine to spawn orbs
        IEnumerator SpawnOrbs( LayoutList layout ) {
            var index = 0;
            foreach ( var position in layout.positions ) {
                //If index has reached the end
            
                if ( index < bagOfOrbs.Count ) {
                    var orb = bagOfOrbs[index];
                    PlaceOrb( position, orb );
                }
                index++;
                yield return new WaitForSeconds( 0.05f );
            }
        }
        
        //Draw gizmos on orbtype.none
        private void OnDrawGizmos() {
            if ( _nodes == null ) return;
            foreach ( var node in _nodes.Values ) {
                if ( node.CurrentOrb is OrbType.NonPlayable ) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube( boardTilemap.CellToWorld( node.Position ), Vector3.one * 0.5f );
                }
            }
        }
    }
    

    [Flags]
    public enum OrbType {
        NonPlayable = 0,
        None = 1,
        Fire = 2,
        Water = 4,
        Earth = 8,
        Air = 16,
        Nuke = 32,
        Zero = 64,
        One = 128,
        Save = 256,
        NetworkOne = 1024,
        NetworkTwo = 2048,
        NetworkThree = 4096,
        NetworkFour = 8192,
        NetworkFive = 16384,
        Encrypt = 32768
    }
}



