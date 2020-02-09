using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcaeaParty
{
    [Serializable]
    public class RewardItem
    {
        public string id;
        public string type;
    }
    [Serializable]
    public class Reward
    {
        public RewardItem[] items;
        public int position;
    }
    [Serializable]
    public class Step
    {
        public int position;
        public double capture;
        public string restrict_id;
        public string restrict_type;
    }
    [Serializable]
    public class WorldMap
    {
        public string map_id;
        public int chapter;
        public long available_from;
        public long available_to;
        public bool is_repeatable;
        public string require_id;
        public string require_type;
        public int require_value;
        public string coordinate;
        public int step_count;
        public int stamina_cost;
        public string custom_bg;
        public bool is_available;
        public bool is_legacy;
        public int curr_position;
        public double curr_capture;
        public bool is_locked;
        public Reward[] rewards;
        public Step[] steps;
    }
}
