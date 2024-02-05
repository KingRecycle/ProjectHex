using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace CharlieMadeAThing.ProjectHex {
    [CreateAssetMenu( fileName = "New Orb Type Data", menuName = "ProjectHex/New Orb Type Data", order = 0 )]
    public class OrbTypeData : ScriptableObject {
        [SerializedDictionary]
        public SerializedDictionary<OrbType, GameObject> orbTypeToPrefab;
    }
}