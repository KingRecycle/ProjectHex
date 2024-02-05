using UnityEngine;

namespace CharlieMadeAThing.ProjectHex {
    [CreateAssetMenu( fileName = "New Orb Data", menuName = "ProjectHex/New Orb Data", order = 0 )]
    public class OrbData : ScriptableObject {
        public OrbType CurrentOrbType;
        public OrbType MatchingTypes;
    }
}