using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class XUiC_BeansTotalCraftingTime : XUiController
{
    private float _refreshTimer = 0,
                  _lastTotalTime = 0;

    private XUiC_CraftingQueue _parentCraftingQueue;

    private const float THROTTLE_TIME = .1f;
    public override void Init()
    {
        base.Init();
        _parentCraftingQueue = parent as XUiC_CraftingQueue;
    }

    public override void Update(float _dt)
    {
        base.Update(_dt);
        
        // Throttle it
        _refreshTimer += _dt;
        if (_refreshTimer < THROTTLE_TIME) return;
        
        _refreshTimer = 0;
        
        if (!XUi.IsGameRunning()) return;

        // Do expensive stuff
        UpdateTotalTime();
    }

    private void UpdateTotalTime()
    {
        if (_parentCraftingQueue != null)
        {
            XUiC_RecipeStack[] recipes = _parentCraftingQueue.GetRecipesToCraft();

            if (recipes != null)
            {
                var totalTime = recipes.Sum(e => (e?.GetTotalRecipeCraftingTimeLeft() ?? 0));
                if (totalTime != _lastTotalTime)
                {
                    _lastTotalTime = totalTime;
                    RefreshBindings(true);
                }
            }
        }
    }

    public override bool GetBindingValue(ref string _value, string _bindingName)
    {
        switch (_bindingName)
        {
            case "total_craft_time":
                {
                    _value = $"{Math.Floor(_lastTotalTime / 60):0}:{(_lastTotalTime % 60):00}";
                    return true;
                }
            default:
                break;
        }
        return base.GetBindingValue(ref _value, _bindingName);
    }
}
