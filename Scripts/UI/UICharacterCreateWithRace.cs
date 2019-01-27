using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace MultiplayerARPG
{
    public class UICharacterCreateWithRace : UICharacterCreate
    {
        public CharacterRaceTogglePair[] raceToggles;

        private Dictionary<CharacterRace, Toggle> cacheRaceToggles;
        public Dictionary<CharacterRace, Toggle> CacheRaceToggles
        {
            get
            {
                if (cacheRaceToggles == null)
                {
                    cacheRaceToggles = new Dictionary<CharacterRace, Toggle>();
                    foreach (CharacterRaceTogglePair raceToggle in raceToggles)
                    {
                        cacheRaceToggles[raceToggle.race] = raceToggle.toggle;
                    }
                }
                return cacheRaceToggles;
            }
        }

        private readonly HashSet<CharacterRace> selectedRaces = new HashSet<CharacterRace>();

        public override void Show()
        {
            foreach (KeyValuePair<CharacterRace, Toggle> raceToggle in CacheRaceToggles)
            {
                raceToggle.Value.onValueChanged.RemoveAllListeners();
                raceToggle.Value.onValueChanged.AddListener((isOn) =>
                {
                    OnRaceToggleUpdate(raceToggle.Key, isOn);
                });
                OnRaceToggleUpdate(raceToggle.Key, raceToggle.Value.isOn);
            }
            base.Show();
        }

        protected override List<BasePlayerCharacterEntity> GetCreatableCharacters()
        {
            return GameInstance.PlayerCharacterEntities.Values.Where((a) => selectedRaces.Contains(a.race)).ToList();
        }

        private void OnRaceToggleUpdate(CharacterRace race, bool isOn)
        {
            if (isOn)
            {
                selectedRaces.Add(race);
                LoadCharacters();
            }
            else
                selectedRaces.Remove(race);
        }
    }
}
