using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun
{
    public abstract float damage { get; }
    public abstract float fireRate { get; }
    public abstract float reloadTime { get; }
    public abstract int penetration { get; }
    public abstract int maxAmmo { get; }
    public abstract int ammoRemaining { get; set; }
    public abstract int damageUpgradeLevel { get; set; }
    public abstract int reloadUpgradeLevel { get; set; }
    public abstract int penetrationUpgradeLevel { get; set; }
    public abstract int maxDamageUpgradeLevel { get; }
    public abstract int maxReloadUpgradeLevel { get; }
    public abstract int maxPenetrationUpgradeLevel { get; }


    public abstract void upgradeDamage();
    public abstract void upgradePenetration();
    public abstract void upgradeReload();

    public abstract void ResetStats();


}
public class Pistol : Gun
{
    public override float damage => 8f + damageUpgradeLevel * 1f; 
    public override float fireRate => 3f; //bullets per second?
    public override float reloadTime => 0.6f - (0.045f * reloadUpgradeLevel); 
    public override int penetration => 1 + penetrationUpgradeLevel;
    public override int maxAmmo => 12;
    private int _ammoRemaining;
    private int _damageUpgradeLevel;
    private int _reloadUpgradeLevel;
    private int _penetrationUpgradeLevel;
    public override int maxDamageUpgradeLevel => 12;
    public override int maxReloadUpgradeLevel => 12;
    public override int maxPenetrationUpgradeLevel => 12;

    public override int ammoRemaining
    {
        get { return _ammoRemaining; }
        set { _ammoRemaining = value; }
    }
    public override int damageUpgradeLevel
    {
        get { return _damageUpgradeLevel; }
        set { _damageUpgradeLevel = value; }
    }
    
    public override int reloadUpgradeLevel
    {
        get { return _reloadUpgradeLevel; }
        set { _reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return _penetrationUpgradeLevel; }
        set { _penetrationUpgradeLevel = value; }
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

    public override void ResetStats()
    {
        damageUpgradeLevel = 0;
        reloadUpgradeLevel = 0;
        penetrationUpgradeLevel = 0;
        _ammoRemaining = maxAmmo;
    }
}

public class Machinegun : Gun
{
    public override float damage => 6f + damageUpgradeLevel * 0.5f;
    public override float fireRate => 9f;
    public override float reloadTime => 3f - (0.12f * reloadUpgradeLevel);
    public override int penetration => 1 + penetrationUpgradeLevel;
    public override int maxAmmo => 24;
    private int _damageUpgradeLevel;
    private int _reloadUpgradeLevel;
    private int _penetrationUpgradeLevel;
    private int _ammoRemaining;
    public override int maxDamageUpgradeLevel => 6;
    public override int maxReloadUpgradeLevel => 6;
    public override int maxPenetrationUpgradeLevel => 6;

    public override int ammoRemaining
    {
        get { return _ammoRemaining; }
        set { _ammoRemaining = value; }
    }
    public override int damageUpgradeLevel
    {
        get { return _damageUpgradeLevel; }
        set { _damageUpgradeLevel = value; }
    }

    public override int reloadUpgradeLevel
    {
        get { return _reloadUpgradeLevel; }
        set { _reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return _penetrationUpgradeLevel; }
        set { _penetrationUpgradeLevel = value; }
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

    public override void ResetStats()
    {
        damageUpgradeLevel = 0;
        reloadUpgradeLevel = 0;
        penetrationUpgradeLevel = 0;
        _ammoRemaining = maxAmmo;
    }
}

public class Shotgun : Gun
{
    public override float damage => 6f + 0.4f * damageUpgradeLevel; //Low damage cause multiple bullets
    public override float fireRate => 3f;
    public override float reloadTime => 2f - (0.1f * reloadUpgradeLevel);
    public override int penetration => 2 + penetrationUpgradeLevel;
    public override int maxAmmo => 4;
    private int _damageUpgradeLevel;
    private int _reloadUpgradeLevel;
    private int _penetrationUpgradeLevel;
    private int _ammoRemaining;
    public override int maxDamageUpgradeLevel => 6;
    public override int maxReloadUpgradeLevel => 6;
    public override int maxPenetrationUpgradeLevel => 6;

    public override int ammoRemaining
    {
        get { return _ammoRemaining; }
        set { _ammoRemaining = value; }
    }
    public override int damageUpgradeLevel
    {
        get { return _damageUpgradeLevel; }
        set { _damageUpgradeLevel = value; }
    }

    public override int reloadUpgradeLevel
    {
        get { return _reloadUpgradeLevel; }
        set { _reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return _penetrationUpgradeLevel; }
        set { _penetrationUpgradeLevel = value; }
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

    public override void ResetStats()
    {
        damageUpgradeLevel = 0;
        reloadUpgradeLevel = 0;
        penetrationUpgradeLevel = 0;
        _ammoRemaining = maxAmmo;
    }
}

public class Rocketlauncher : Gun
{
    public override float damage => 8f + 2f * damageUpgradeLevel; //Should do "aoe"
    public override float fireRate => 0.8f;
    public override float reloadTime => 2f - (0.2f * reloadUpgradeLevel);
    public override int penetration => 7 + penetrationUpgradeLevel;
    public override int maxAmmo => 3;
    private int _damageUpgradeLevel;
    private int _reloadUpgradeLevel;
    private int _penetrationUpgradeLevel;
    private int _ammoRemaining;
    public override int maxDamageUpgradeLevel => 6;
    public override int maxReloadUpgradeLevel => 6;
    public override int maxPenetrationUpgradeLevel => 6;
    public override int ammoRemaining
    {
        get { return _ammoRemaining; }
        set { _ammoRemaining = value; }
    }

    public override int damageUpgradeLevel
    {
        get { return _damageUpgradeLevel; }
        set { _damageUpgradeLevel = value; }
    }

    public override int reloadUpgradeLevel
    {
        get { return _reloadUpgradeLevel; }
        set { _reloadUpgradeLevel = value; }
    }
    public override int penetrationUpgradeLevel
    {
        get { return _penetrationUpgradeLevel; }
        set { _penetrationUpgradeLevel = value; }
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

    public override void ResetStats()
    {
        damageUpgradeLevel = 0;
        reloadUpgradeLevel = 0;
        penetrationUpgradeLevel = 0;
        _ammoRemaining = maxAmmo;
    }
}
