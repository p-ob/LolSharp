namespace LolSharp.RiotDtos.StaticData
{
    using System.Collections.Generic;

    public class ChampionSpellDto
    {
        public List<ImageDto> AltImages { get; set; }
        public List<double> Cooldown { get; set; }
        public string CooldownBurn { get; set; }
        public List<int> Cost { get; set; }
        public string CostBurn { get; set; }
        public string CostType { get; set; }
        public string Description { get; set; }
        public List<object> Effect { get; set; }
        public List<string> EffectBurn { get; set; }
        public ImageDto Image { get; set; }
        public string Key { get; set; }
        public LevelTipDto LevelTip { get; set; }
        public int MaxRank { get; set; }
        public string Name { get; set; }
        public object Range { get; set; }
        public string RangeBurn { get; set; }
        public string Resource { get; set; }
        public string SanitizedDescription { get; set; }
        public string SanitizedTooltip { get; set; }
        public string Tooltip { get; set; }
        public List<SpellVarsDto> Vars { get; set; }
    }
}