using System;
using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Balancing;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.CalPlayer;
using CalamityMod.Dusts;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Armor.Daedalus;
using CalamityMod.Items.Armor.Demonshade;
using CalamityMod.Items.Armor.Empyrean;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Hydrothermic;
using CalamityMod.Items.Armor.Reaver;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.NPCs.Other;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FaultCombat;

[JITWhenModsEnabled("CalamityMod")]
public class FaultCalamityPlayerData
{
    public static FaultCalamityPlayerData Load()
    {
        if (ModLoader.GetMod("CalamityMod") == null) return null;
        var data = new FaultCalamityPlayerData();
        return data;
    }

    public bool yharimGift;

    public void ResetEffects()
    {
        yharimGift = false;
    }
    public void GrandGelatinTooltips(List<TooltipLine> tooltips,FaultPlayer fp)
    {
        var clam = fp.Player.Calamity();
        if (clam.GrandGelatin) tooltips.Add(new TooltipLine(fp.Mod,"Fault Combat : Grand Gelatin","[i:CalamityMod/GrandGelatin] Grant single extra dodgeroll"));
    }
    public void GrandGelatinEffect(FaultPlayer fp)
    {
        var clam = fp.Player.Calamity();
        if (clam.GrandGelatin)
        {
            fp.extraRoll = true;
            CombatText.NewText(fp.Player.Hitbox,Color.Pink,"Granted Extra Dodgeroll");
        }
    }
    public void PostUpdateEquip(FaultPlayer faultPlayer, Player Player)
    {
        var clam = Player.Calamity();

        if (clam.angelTreads)
        {
            faultPlayer.statRollCooldown -= 0.1f;
            faultPlayer.statMaxStamina += 2f;
        }
        else if(clam.moonWalkers)
        {
            faultPlayer.statRollCooldown -= 0.1f;
            faultPlayer.statMaxStamina += 3f;
        }
        else if(clam.voidStriders)
        {
            faultPlayer.statRollCooldown -= 0.1f;
            faultPlayer.statMaxStamina += 4f;
        }
        else if(clam.seraphTracers)
        {
            faultPlayer.statRollCooldown -= 0.1f;
            faultPlayer.statMaxStamina += 5f;
        }
    }

    public void HurtEffect_Local(Player.HurtInfo info, CalamityPlayer clam, Player Player)
    {
        // Summon a portal if needed. EDIT : wtf is the demon portal bru
        // if (Player.Calamity().persecutedEnchant)
        // {
        //     if (NPC.CountNPCS(ModContent.NPCType<DemonPortal>()) < 2)
        //     {
        //         int tries = 0;
        //         Vector2 spawnPosition;
        //         Vector2 spawnPositionOffset = Vector2.One * 24f;
        //         do
        //         {
        //             spawnPosition = Player.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(270f, 420f);
        //             tries++;
        //         }
        //         while (Collision.SolidCollision(spawnPosition - spawnPositionOffset, 48, 24) && tries < 100);
        //         CalamityNetcode.NewNPC_ClientSide(spawnPosition, ModContent.NPCType<DemonPortal>(), Player);
        //     }
        // }

        // Armor Sets Buff
        if (clam.tarraMelee)
        {
            Player.AddBuff(ModContent.BuffType<TarraLifeRegen>(), TarragonHeadMelee.TarraLifeDuration / 2);
        }
        else if (clam.xerocSet)
        {
            Player.AddBuff(ModContent.BuffType<EmpyreanWrath>(), EmpyreanMask.WrathDuration / 2);
        }
        else if (clam.reaverDefense)
        {
            Player.AddBuff(ModContent.BuffType<ReaverRage>(), ReaverHeadTank.ReaverRageDuration / 2);
        }

        // aquatic heart
        if (clam.fBarrier || (clam.aquaticHeart && NPC.downedBoss3))
        {
            SoundEngine.PlaySound(SoundID.Item27, Player.Center);
            foreach (NPC targetNPC in Main.ActiveNPCs)
            {
                if (targetNPC.friendly || targetNPC.dontTakeDamage)
                    continue;

                float npcDist = (targetNPC.Center - Player.Center).Length();
                float freezeDist = 300 + (int)info.Damage * 2;
                if (freezeDist > 500f)
                    freezeDist = 500f + (freezeDist - 500f) * 0.5f;

                if (npcDist < freezeDist)
                {
                    float duration = Main.rand.Next(10 + (int)info.Damage / 2, 20 + (int)info.Damage);
                    if (duration > 120)
                        duration = 120;

                    targetNPC.AddBuff(ModContent.BuffType<GlacialState>(), (int)duration, false);
                }
            }
        }

        // By setting brainOfConfusionItem, these accessories have this code already,
        // but doing it again allows for increased duration + The Amalgam's other buffs,
        // and also doesn't have random chance (why does Brain of Confusion not guarantee confusion on hit)
        if (clam.aBrain || clam.amalgam)
        {
            foreach (NPC targetNPC in Main.ActiveNPCs)
            {
                if (targetNPC.friendly || targetNPC.dontTakeDamage)
                    continue;

                float npcDist = (targetNPC.Center - Player.Center).Length();
                float range = Main.rand.Next(200 + (int)info.Damage / 2, 301 + (int)info.Damage * 2);
                if (range > 500f)
                    range = 500f + (range - 500f) * 0.75f;
                if (range > 700f)
                    range = 700f + (range - 700f) * 0.5f;
                if (range > 900f)
                    range = 900f + (range - 900f) * 0.25f;

                if (npcDist < range)
                {
                    int duration = Main.rand.Next(300 + info.Damage / 3, 480 + info.Damage / 2);
                    targetNPC.AddBuff(BuffID.Confused, duration, false);
                }
            }

            // Spawn the harmless brain images that are actually projectiles
            var source = Player.GetSource_Accessory_OnHurt(clam.amalgam ? clam.FindAccessory<TheAmalgam>() : clam.FindAccessory<AmalgamatedBrain>(), info.DamageSource);
            Projectile.NewProjectile(source, Player.Center.X + Main.rand.Next(-40, 40), Player.Center.Y - Main.rand.Next(20, 60), Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f, ProjectileID.BrainOfConfusion, 0, 0f, Player.whoAmI);
        }

        // On Hit By NPC
        if (!info.DamageSource.TryGetCausingEntity(out Entity entity)) return;
        if (entity is NPC npc && npc.active && !npc.friendly)
        {
            if (clam.crawCarapace)
            {
                npc.AddBuff(ModContent.BuffType<Crumbling>(), 900);
                Vector2 pushVel = Utils.DirectionTo(Player.Center, npc.Center) * 7;
                if (!npc.dontTakeDamage)
                {
                    int onHitDamage = (int)Player.GetBestClassDamage().ApplyTo(CrawCarapace.ThornsDamage);
                    Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(clam.FindAccessory<CrawCarapace>(), npc), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), onHitDamage, 0f, Player.whoAmI, npc.whoAmI, pushVel.X, pushVel.Y);
                }
                SoundEngine.PlaySound(SoundID.NPCHit33 with { Volume = 0.5f }, Player.Center);

                for (int i = 0; i < 10; i++)
                {
                    float accuracy = Main.rand.NextFloat(-0.4f, 0.4f);
                    float powerMult = (1 - Math.Abs(accuracy));

                    Vector2 dustVel = (pushVel).SafeNormalize(Vector2.UnitY).RotatedBy(accuracy * 2) * Main.rand.NextFloat(4, 7) * powerMult;
                    Vector2 fxPos = Player.Center + dustVel;

                    Dust dust = Dust.NewDustPerfect(fxPos, Main.rand.NextBool(4) ? 249 : 115, dustVel, 0, default, Main.rand.NextFloat(0.75f, 1.2f));
                    dust.noGravity = true;
                    dust.noGravity = true;
                    dust.fadeIn = 1f;
                }
            }

            if (clam.baroclaw)
            {
                Vector2 pushVel = Utils.DirectionTo(Player.Center, npc.Center) * 15;
                if (!npc.dontTakeDamage)
                {
                    int onHitDamage = (int)Player.GetBestClassDamage().ApplyTo(Baroclaw.ThornsDamage);
                    Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(clam.FindAccessory<Baroclaw>(), npc), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), onHitDamage, -1f, Player.whoAmI, npc.whoAmI, pushVel.X, pushVel.Y);

                    npc.AddBuff(ModContent.BuffType<ArmorCrunch>(), 900);
                    npc.AddBuff(ModContent.BuffType<CrushDepth>(), 900);
                }
                //SoundEngine.PlaySound(BaroclawHit, Player.Center);

                for (int i = 0; i < 17; i++)
                {
                    float accuracy = Main.rand.NextFloat(-0.55f, 0.55f);
                    float powerMult = 1 - Math.Abs(accuracy);
                    Vector2 fxVel = pushVel.SafeNormalize(Vector2.UnitY).RotatedBy(accuracy) * Main.rand.NextFloat(5, 12) * powerMult;
                    Vector2 dustVel = pushVel.SafeNormalize(Vector2.UnitY).RotatedBy(accuracy * 2) * Main.rand.NextFloat(10, 20) * powerMult;
                    Vector2 fxPos = Player.Center + fxVel;
                    Color fxColor = Color.Lerp(Color.RoyalBlue, Color.DarkBlue, Main.rand.NextFloat(1f));

                    Particle fx = new CustomSpark(fxPos, fxVel, "CalamityMod/Particles/PointParticle", false, (int)(Main.rand.Next(22, 40 + 1) * powerMult), Main.rand.NextFloat(1.95f, 2.2f) * powerMult, fxColor, new Vector2(0.5f, 1.1f), extraRotation: 0, shrinkSpeed: Main.rand.NextFloat(0.1f, 0.3f) + (1 - powerMult) * 0.3f);
                    GeneralParticleHandler.SpawnParticle(fx);

                    if (i % 3 == 0)
                    {
                        Dust dust = Dust.NewDustPerfect(fxPos, DustID.FireworksRGB, dustVel, 0, default, Main.rand.NextFloat(0.75f, 1.1f));
                        dust.noGravity = true;
                        dust.color = Color.Gold;
                        dust.noGravity = false;
                    }
                }
            }

            if (clam.absorber)
            {
                Vector2 pushVel = Utils.DirectionTo(Player.Center, npc.Center) * 22;
                if (!npc.dontTakeDamage)
                {
                    int onHitDamage = (int)Player.GetBestClassDamage().ApplyTo(TheAbsorber.ThornsDamage);
                    Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(clam.FindAccessory<TheAbsorber>(), npc), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), onHitDamage, -1f, Player.whoAmI, npc.whoAmI, pushVel.X, pushVel.Y);

                    npc.AddBuff(ModContent.BuffType<AbsorberAffliction>(), 900);
                }
                //SoundEngine.PlaySound(AbsorberHit, Player.Center);

                for (int i = 0; i < 25; i++)
                {
                    float accuracy = Main.rand.NextFloat(-0.7f, 0.7f);
                    float powerMult = (1 - Math.Abs(accuracy));
                    Vector2 fxVel = (pushVel).SafeNormalize(Vector2.UnitY).RotatedBy(accuracy) * Main.rand.NextFloat(10, 18) * powerMult;
                    Vector2 dustVel = (pushVel).SafeNormalize(Vector2.UnitY).RotatedBy(accuracy * 2) * Main.rand.NextFloat(15, 30) * powerMult;
                    Vector2 fxPos = Player.Center + fxVel;
                    Color fxColor = Color.Lerp(Color.DarkSeaGreen, Color.MediumSeaGreen, Main.rand.NextFloat(1f));

                    Particle fx = new CustomSpark(fxPos, fxVel, "CalamityMod/Particles/Sparkle", false, (int)(Main.rand.Next(32, 50 + 1) * powerMult), Main.rand.NextFloat(2.25f, 2.5f) * powerMult, fxColor, new Vector2(0.5f, 1.1f), extraRotation: 0, shrinkSpeed: Main.rand.NextFloat(0.1f, 0.3f) + (1 - powerMult) * 0.3f);
                    GeneralParticleHandler.SpawnParticle(fx);

                    Dust dust = Dust.NewDustPerfect(fxPos, ModContent.DustType<LightDust>(), dustVel, 0, default, Main.rand.NextFloat(0.95f, 2.1f));
                    dust.noGravity = true;
                    dust.color = fxColor;
                }
            }
        }
    }
    public void HurtEffect(Player.HurtInfo info, Player Player)
    {
        var clam = Player.Calamity();

        // The transformer , Hide of Astrum Deus , Frost Barrier, Craw Carapace , Deific Amulet 
        // Void of calamity , Yharim Gift , Amidias Spark, Alchemical Flask, Rampart of Deities

        clam.shouldTriggerBeeCooldown = false;
        clam.OnHitByCombat(info); // alchFlask , ursaSergeant , corrosiveSpine

        if (Main.dedServ && Player.whoAmI == Main.myPlayer)
        {
            HurtEffect_Local(info, clam, Player);
            ClamPostHurt(info, clam, Player);
            ClamOnHurt(clam, Player, info);
        }

        if (yharimGift)
        {
            IEntitySource source = Player.GetSource_Accessory_OnHurt(null, info.DamageSource);
            int damage2 = (int)Player.GetBestClassDamage().ApplyTo(375f);
            CalamityUtils.ProjectileRain(source, Player.Center, 400f, 100f, 500f, 800f, 22f, ModContent.ProjectileType<SkyFlareFriendly>(), damage2, 9f, Player.whoAmI);
        }
    }

    public void ClamPostHurt(Player.HurtInfo hurtInfo, CalamityPlayer clam, Player Player)
    {
        //This goes before the canTriggerEffects check on purpose to match Honeycomb
        if (clam.dAmulet) Player.AddBuff(BuffID.Honey, 300, false);

        // Handle hit effects from the gem tech armor set.
        Player.Calamity().GemTechState.PlayerOnHitEffects((int)hurtInfo.Damage);

        if (clam.aeroSet && hurtInfo.Damage > AerospecBreastplate.SetBonusHurtDamageThreshold)
        {
            // https://github.com/tModLoader/tModLoader/wiki/IEntitySource#detailed-list
            var source = Player.GetSource_OnHurt(hurtInfo.DamageSource, "Aerospec Breastplate");
            int featherDamage = (int)Player.GetBestClassDamage().ApplyTo(AerospecBreastplate.SetBonusFeatherDamage / 2);
            for (int n = 0; n < 4; n++)
            {
                CalamityUtils.ProjectileRain(source, Player.Center, 400f, 100f, 500f, 800f, 20f, ModContent.ProjectileType<StickyFeatherAero>(), featherDamage, 1f, Player.whoAmI);
            }
        }
        if (clam.hideOfDeus)
        {
            var source = Player.GetSource_Accessory_OnHurt(clam.FindAccessory<HideofAstrumDeus>(), hurtInfo.DamageSource);
            SoundEngine.PlaySound(SoundID.Item74, Player.Center);

            int blazeDamage = (int)Player.GetBestClassDamage().ApplyTo(HideofAstrumDeus.BlazeDamage / 2);
            Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, 0f, 0f, ModContent.ProjectileType<HideOfAstrumDeusExplosion>(), blazeDamage, 5f, Player.whoAmI, 0f, 1f);
        }
        // TODO -- Make Deific Amulet and Rampart of Deities' retaliation effects way cooler
        // In the meantime, gave them homing astral star bees instead of the lame falling stars.
        // This also serves to make the Honeycomb in Sweetheart Necklace make sense
        if (clam.dAmulet)
        {
            var source = Player.GetSource_Accessory_OnHurt(clam.FindAccessory<DeificAmulet>(), hurtInfo.DamageSource);
            int projAmount = (clam.rampartOfDeities ? 12 : 6);
            for (int n = 0; n < projAmount; n++)
            {
                int deificProjDamage = (int)(Player.GetBestClassDamage().ApplyTo(DeificAmulet.StarDamage / 2) * (Player.strongBees ? 0.85f : 1f));

                Projectile onHitProj = Main.projectile[Projectile.NewProjectile(source, Player.Center, new Vector2(0, -15 * (clam.rampartOfDeities && n % 2 == 0 ? 0.75f : 1.25f)).RotatedBy(MathHelper.TwoPi / projAmount * n), ModContent.ProjectileType<AstralStar>(), deificProjDamage, 4f, Player.whoAmI)];
                if (onHitProj.whoAmI.WithinBounds(Main.maxProjectiles))
                {
                    onHitProj.DamageType = DamageClass.Generic;
                    onHitProj.usesLocalNPCImmunity = true;
                    onHitProj.localNPCHitCooldown = 30;
                    onHitProj.tileCollide = false;
                    onHitProj.extraUpdates = 1;
                    onHitProj.Calamity().conditionalHomingRange = 600f;
                    if (Player.strongBees)
                        onHitProj.penetrate += 1;
                }
            }
        }
        if (clam.ilSpark)
        {
            var source = Player.GetSource_Accessory(clam.FindAccessory(ModContent.ItemType<HideofAstrumDeus>()));
            if (hurtInfo.Damage > 0)
            {
                SoundEngine.PlaySound(SoundID.Item93, Player.Center);
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;

                // Start with base damage, then apply the best damage class you can
                int sDamage = 6;
                if (clam.transformer) sDamage += 42;

                sDamage = (int)Player.GetBestClassDamage().ApplyTo(sDamage / 2);

                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    int spark1 = Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<GenericElectricSpark>(), sDamage, 1.25f, Player.whoAmI, 0f, 1);
                    int spark2 = Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<GenericElectricSpark>(), sDamage, 1.25f, Player.whoAmI, 0f, 1);
                    if (spark1.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[spark1].timeLeft = 120;
                    }
                    if (spark2.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[spark2].timeLeft = 120;
                    }
                }

            }
        }
        if (clam.rBrain)
        {
            if (!(CalamityUtils.AnyProjectiles(ModContent.ProjectileType<ShadeNimbus>()) || CalamityUtils.AnyProjectiles(ModContent.ProjectileType<ShadeNimbusSpawner>())))
            {
                var source = Player.GetSource_Accessory_OnHurt(clam.amalgam ? clam.FindAccessory<TheAmalgam>() : clam.aBrain ? clam.FindAccessory<AmalgamatedBrain>() : clam.FindAccessory<RottenBrain>(), hurtInfo.DamageSource);
                int effectStrength = clam.amalgam ? 3 : clam.aBrain ? 2 : 1;
                int effectDamage = clam.amalgam ? TheAmalgam.NimbusDamage : clam.aBrain ? AmalgamatedBrain.NimbusDamage : RottenBrain.NimbusDamage;
                effectDamage = (int)Player.GetBestClassDamage().ApplyTo(effectDamage / 2);

                Vector2 spawnerVelocity = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 40f) * 12.5f;
                Projectile.NewProjectile(source, Player.Center, spawnerVelocity, ModContent.ProjectileType<ShadeNimbusSpawner>(), effectDamage, 0f, Player.whoAmI, 0f, 0f, effectStrength);
            }
        }

        // Seems like this is a visual only shi
        // if (clam.inkBomb && !clam.abyssalMirror && !clam.eclipseMirror)
        // {
        //     var source = Player.GetSource_Accessory_OnHurt(clam.FindAccessory<CalamityMod.Items.Accessories.InkBomb>(), hurtInfo.DamageSource);
        //     SoundEngine.PlaySound(SoundID.Item1, Player.Center);
        //     for (int i = 0; i < 3; i++)
        //     {
        //         int ink = Projectile.NewProjectile(source, Player.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 2f, 
        //         ModContent.ProjectileType<InkBombProjectile>(), 0, 0, Player.whoAmI);
        //         if (ink.WithinBounds(Main.maxProjectiles))
        //             Main.projectile[ink].DamageType = DamageClass.Generic;
        //     }
        // }

        if (clam.ataxiaBlaze)
        {
            // fuck off
            var fuckYouBitch = Player.GetSource_OnHurt(hurtInfo.DamageSource);
            if (hurtInfo.Damage > 0)
            {
                SoundEngine.PlaySound(SoundID.Item74, Player.Center);
                int eDamage = (int)Player.GetBestClassDamage().ApplyTo(HydrothermicArmor.BlazeDamage / 2);

                if (Player.whoAmI == Main.myPlayer)
                    Projectile.NewProjectile(fuckYouBitch, Player.Center, Vector2.Zero, ModContent.ProjectileType<DeepseaBlaze>(), eDamage, 1f, Player.whoAmI, 0f, 0f);
            }
        }
        else if (clam.daedalusShard) // Daedalus Ranged helm
        {
            SoundEngine.PlaySound(SoundID.Item27, Player.Center);

            var source = Player.GetSource_OnHurt(hurtInfo.DamageSource);
            float offset = Main.rand.NextFloat(MathHelper.TwoPi);
            int sDamage = (int)Player.GetTotalDamage<RangedDamageClass>().ApplyTo(DaedalusHeadRanged.ShardDamage / 2);
            for (int i = 0; i < 10; i++)
            {
                Vector2 circleVel = ((MathHelper.TwoPi * i / 10f) + offset).ToRotationVector2() * Main.rand.NextFloat(5f, 8f);
                int shard = Projectile.NewProjectile(source, Player.Center, circleVel, ProjectileID.CrystalShard, sDamage, 1f, Player.whoAmI);
                if (shard.WithinBounds(Main.maxProjectiles))
                    Main.projectile[shard].DamageType = DamageClass.Generic;
            }

        }
        else if (clam.godSlayerDamage) //god slayer melee helm
        {
            var source = Player.GetSource_OnHurt(hurtInfo.DamageSource);

            SoundEngine.PlaySound(SoundID.Item73, Player.Center);
            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int shrapnelDamage = Player.CalcIntDamage<MeleeDamageClass>(GodSlayerHeadMelee.DartDamage / 2);
            if (Player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<GodKiller>(), shrapnelDamage, 5f, Player.whoAmI, 0f, 0f);
                    Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<GodKiller>(), shrapnelDamage, 5f, Player.whoAmI, 0f, 0f);
                }
            }

        }
        else if (clam.dsSetBonus)
        {
            // https://github.com/tModLoader/tModLoader/wiki/IEntitySource#detailed-list
            var source = Player.GetSource_OnHurt(hurtInfo.DamageSource, "DemonshadeHelm.ShadowScytheEntitySourceContext");
            for (int l = 0; l < 2; l++)
            {
                int shadowbeamDamage = (int)Player.GetBestClassDamage().ApplyTo(DemonshadeHelm.BeamDamage / 2);

                Projectile beam = CalamityUtils.ProjectileRain(source, Player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileID.ShadowBeamFriendly, shadowbeamDamage, 7f, Player.whoAmI);
                if (beam.whoAmI.WithinBounds(Main.maxProjectiles))
                {
                    beam.DamageType = DamageClass.Generic;
                    beam.usesLocalNPCImmunity = true;
                    beam.localNPCHitCooldown = 10;
                }
            }
            for (int l = 0; l < 5; l++)
            {
                int scytheDamage = (int)Player.GetBestClassDamage().ApplyTo(DemonshadeHelm.ScytheDamage / 2);

                Projectile scythe = CalamityUtils.ProjectileRain(source, Player.Center, 400f, 100f, 500f, 800f, 22f, ProjectileID.DemonScythe, scytheDamage, 7f, Player.whoAmI);
                if (scythe.whoAmI.WithinBounds(Main.maxProjectiles))
                {
                    scythe.DamageType = DamageClass.Generic;
                    scythe.usesLocalNPCImmunity = true;
                    scythe.localNPCHitCooldown = 10;
                }
            }
        }
    }

    public void ClamOnHurt(CalamityPlayer clam, Player Player, Player.HurtInfo hurtInfo)
    {
        #region Shattered Community Rage Gain
        // Shattered Community makes the player gain rage based on the amount of damage taken.
        // Also set the Rage gain cooldown to prevent bizarre abuse cases.
        if (clam.shatteredCommunity && clam.rageGainCooldown == 0)
        {
            float HPRatio = 100f / Player.statLifeMax2;
            float rageConversionRatio = 0.8f;

            // Damage to rage conversion is half as effective while Rage Mode is active.
            if (clam.rageModeActive)
                rageConversionRatio *= 0.5f;
            // If Rage is over 100%, damage to rage conversion scales down asymptotically based on how full Rage is.
            if (clam.rage >= clam.rageMax)
                rageConversionRatio *= 3f / (3f + clam.rage / clam.rageMax);

            clam.rage += clam.rageMax * HPRatio * rageConversionRatio;
            clam.rageGainCooldown = ShatteredCommunity.RageGainCooldown;
            // Rage capping is handled in MiscEffects
        }
        #endregion

        // Huge question, why tf does this broke
        // if (clam.RageEnabled) clam.rageCombatFrames = BalancingConstants.RageCombatDelayTime;

        // Hide of Astrum Deus' melee boost
        if (clam.hideOfDeus)
        {
            clam.hideOfDeusMeleeBoostTimer += 3 * 50;
            if (clam.hideOfDeusMeleeBoostTimer > 600)
                clam.hideOfDeusMeleeBoostTimer = 600;
        }
    }

    public void CheckDodge(Player player, PlayerDeathReason source, ref bool dodgeable)
    {
        var clam = player.Calamity();
        if (clam.blazingCursorVisuals)
        {
            dodgeable = true;
            return;
        }
        if (clam.angelTreads)
        {
            if (Main.rand.NextBool(40)) dodgeable = true;
            return;
        }
        else if (clam.moonWalkers)
        {
            if (Main.rand.NextBool(30)) dodgeable = true;
            return;
        }
        else if (clam.voidStriders)
        {
            if (Main.rand.NextBool(20)) dodgeable = true;
            return;
        }
        else if (clam.seraphTracers)
        {
            if (Main.rand.NextBool(10)) dodgeable = true;
            return;
        }
    }
}