using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Holoraria.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class GuraLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
			Item.height = 20;
			Item.value = 1500;
			Item.rare = 1;
			Item.defense = 13;
        }

		 public override void UpdateEquip(Player player)
        {
			player.moveSpeed+= 0.5f;
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
