using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Holoraria.Items.Projectiles
{
	public class AmeBulletProjectile : ModProjectile
	{

		public override void SetDefaults() 
		{
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 4;
			Projectile.height = 20;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 400;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.scale = 0.7f;
			Projectile.extraUpdates = 1;
		}
        

		int bounce = 0;
		int maxBounces = 5;

         int hitCounter = 31; // Initial hit counter value



        public override void AI()
        {
			Projectile.aiStyle = 0;
			Lighting.AddLight(Projectile.position, 0.2f, 0.2f, 0.6f);
			Lighting.Brightness(1, 1);
            Item item = new Item();
            item.SetDefaults(ModContent.ItemType<AmeBullet>());
		}

        public override void Kill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Dig.WithVolumeScale(0.5f).WithPitchOffset(0.8f), Projectile.position);
			for (var i = 0; i < 6; i++)
            {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 7, 0f, 0f, 0, default(Color), 1f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounce++;
			SoundEngine.PlaySound(SoundID.Dig.WithVolumeScale(0.5f).WithPitchOffset(0.8f), Projectile.position);
			for (var i = 0; i < 4; i++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 7, 0f, 0f, 0, default(Color), 1f);
			}
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
			Projectile.aiStyle = 1;

			if (bounce >= maxBounces) return true;
			else return false;
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int randomValue = Main.rand.Next(1, hitCounter+1);

            if (randomValue == hitCounter)
            {
                target.GetGlobalNPC<NPCGlobals>().stun = true;
                target.GetGlobalNPC<NPCGlobals>().stunStart = Main.GameUpdateCount;
            }

            hitCounter--;

            // Reset hit counter and increase stun chance after hitting 239 times
            if (hitCounter <= 0)
            {
                hitCounter = 31;
            }

            if (target.HasBuff(BuffID.Midas))
            {
                // Increase the firing speed of the gun by 10
                Item item = new Item();
                item.SetDefaults(ModContent.ItemType<AmeBullet>());
                
                if (item.shootSpeed  < 1000f){
                    item.shootSpeed += 100f;
                }
                else {
                    item.shootSpeed = 10f;
                }
                
            }

        }
        
    }

    public class NPCGlobals : GlobalNPC
    {
        public bool stun;
        public uint stunStart;
        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            if (stun && Main.GameUpdateCount - stunStart > 180) // 180 updates = 3 seconds (60 updates per second)
            {
                stun = false;
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (stun)
            {
                npc.AddBuff(BuffID.Midas, 1);
                npc.velocity *= 0;
            }
        }
    }
}