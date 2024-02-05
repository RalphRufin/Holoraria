using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Holoraria.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class GuraChestplate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
			Item.height = 20;
			Item.value = 1500;
			Item.rare = 1;
			Item.defense = 20;
        }

		 public override void UpdateEquip(Player player)
        {
		     float damageModifier = 1.3f; // +20% melee damage

            player.GetDamage(DamageClass.Melee) = player.GetDamage(DamageClass.Melee) * damageModifier;
        }


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

		}
    }
