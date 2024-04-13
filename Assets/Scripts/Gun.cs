using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun
{
    public abstract float damage { get; }
    public abstract float fireRate { get; }
    public abstract float reloadTime { get; }
    public abstract int penetration { get; }
    public abstract int clip { get; }
    public abstract int damageUpgradeLevel { get; set; }
    public abstract int reloadUpgradeLevel { get; set; }
    public abstract int penetrationUpgradeLevel { get; set; }


    public abstract void upgradeDamage();
    public abstract void upgradePenetration();
    public abstract void upgradeReload();


}
public class Pistol : Gun
{
    public override float damage => 8f + damageUpgradeLevel * 4f; //50% per level
    public override float fireRate => 3f; //bullets per second?
    public override float reloadTime => 1f - (0.1f * reloadUpgradeLevel); //10% per level
    public override int penetration => 1 + penetrationUpgradeLevel;
    public override int clip => 12;
    public override int damageUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }
    
    public override int reloadUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set {  damageUpgradeLevel= value; }
    }

    public override void upgradeDamage()
    {
        damageUpgradeLevel += 1;
    }

    public override void upgradeReload()
    {
        reloadUpgradeLevel += 1;
    }

    public override void upgradePenetration()
    {
        penetrationUpgradeLevel += 1;
    }
}

public class Machinegun : Gun
{
    public override float damage => 6f + damageUpgradeLevel * 3f;
    public override float fireRate => 8f;
    public override float reloadTime => 2f - (0.2f * reloadUpgradeLevel);
    public override int penetration => 1 + penetrationUpgradeLevel;
    public override int clip => 24;
    public override int damageUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }

    public override int reloadUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }

    public override void upgradeDamage()
    {
        damageUpgradeLevel += 1;
    }

    public override void upgradeReload()
    {
        reloadUpgradeLevel += 1;
    }

    public override void upgradePenetration()
    {
        penetrationUpgradeLevel += 1;
    }
}

public class Shotgun : Gun
{
    public override float damage => 8f + 2f * damageUpgradeLevel; //Low damage cause multiple bullets
    public override float fireRate => 1.5f;
    public override float reloadTime => 3f - (0.3f * reloadUpgradeLevel);
    public override int penetration => 2 + penetrationUpgradeLevel;
    public override int clip => 4;
    public override int damageUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }

    public override int reloadUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }

    public override void upgradeDamage()
    {
        damageUpgradeLevel += 1;
    }

    public override void upgradeReload()
    {
        reloadUpgradeLevel += 1;
    }

    public override void upgradePenetration()
    {
        penetrationUpgradeLevel += 1;
    }
}

public class Rocketlauncher : Gun
{
    public override float damage => 20f + 10f * damageUpgradeLevel; //Should do "aoe"
    public override float fireRate => 1f;
    public override float reloadTime => 2f - (0.2f * reloadUpgradeLevel);
    public override int penetration => 1;
    public override int clip => 3;
    public override int damageUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }

    public override int reloadUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return damageUpgradeLevel; }
        set { damageUpgradeLevel = value; }
    }

    public override void upgradeDamage()
    {
        damageUpgradeLevel += 1;
    }

    public override void upgradeReload()
    {
        reloadUpgradeLevel += 1;
    }

    public override void upgradePenetration()
    {
        penetrationUpgradeLevel += 1;
    }
}
