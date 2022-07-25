using System.Text;
using UnityEngine;

public class CreatureInfo : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _name;
    [SerializeField] private TMPro.TextMeshProUGUI _description;

    public void UpdateInfo(Creature creature)
    {
        _name.text = creature.Schema.name;

        var builder = new StringBuilder();
        builder.Append($"{Language.Instance["health"]}: {creature.Schema.health}/{creature.Schema.maxHealth}");
        builder.AppendLine();

        builder.Append($"{Language.Instance["steps"]}: {creature.Schema.steps}/{creature.Schema.maxSteps}");
        builder.AppendLine();

        if (creature.Schema.points > 0)
        {
            builder.Append($"{Language.Instance["points"]}: {creature.Schema.points}");
            builder.AppendLine();
        }

        for (int i = 0; i < creature.Schema.attributes.Count; i++)
        {
            builder.Append(Language.Instance[creature.Schema.attributes[i]]);
            builder.AppendLine();
        }

        for (int i = 0; i < creature.Schema.passiveAbilities.Count; i++)
        {
            builder.Append(Language.Instance[creature.Schema.passiveAbilities[i].name]);
            builder.AppendLine();
        }

        for (int i = 0; i < creature.Schema.abilities.Count; i++)
        {
            builder.AppendLine();
            builder.Append(Language.Instance.GetAbilityText(creature.Schema.abilities[i]));
            builder.AppendLine();
        }

        _description.text = builder.ToString();
    }
}
