// Do not manually modify this file. This file has been procedurally generated.

using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Equippables", menuName = "ScriptableObjects/Equippables", order = 1)]
public class Equippables : ScriptableObject
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Beards
    {
        _CleanShaven,
        AnchorBeard,
        Balbo,
        ChinCurtain,
        ChinstrapBeard,
        CircleBeard,
        DaliMustache,
        DucktailBeard,
        EnglishMustache,
        Forkbeard,
        FriendlyMuttonChops,
        FuManchu,
        GaribaldiBeard,
        Goatee,
        GoatPatch,
        HandlebarMustache,
        HorseshoeMustache,
        HungarianMustache,
        ImperialMustache,
        MuttonChops,
        NeckBeard,
        NedKellyBeard,
        PencilMustache,
        PyramidMustache,
        Shenandoah,
        Sideburns,
        SoulPatch,
        TheZappa,
        VanDykeBeard,
        VerdiBeard,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum HairStyles
    {
        _Bald,
        Afro,
        BaldCrowned,
        Bangs,
        Beehive,
        BigHair,
        Blowout,
        BobCut,
        Bouffant,
        BowlCut,
        Bun,
        Bunches,
        ButchCut,
        CeasarCut,
        Chonmage,
        CroydonFacelift,
        CurtainedHair,
        DoubleBuns,
        DuckTail,
        FeatheredHair,
        FrenchBraid,
        FringeBangs,
        HalfUpdo,
        HighAndTight,
        HimeCut,
        JheriCurl,
        LibertySpikes,
        Lob,
        MarcelWaves,
        Mohawk,
        MopTop,
        Mullet,
        PIxieCut,
        Pompadour,
        PonyTail,
        ShagCut,
        Tonsure,
        TopFade,
    }

    [System.Serializable]
    public struct BeardsPrefabs
    {
        public GameObject _CleanShaven;
        public GameObject AnchorBeard;
        public GameObject Balbo;
        public GameObject ChinCurtain;
        public GameObject ChinstrapBeard;
        public GameObject CircleBeard;
        public GameObject DaliMustache;
        public GameObject DucktailBeard;
        public GameObject EnglishMustache;
        public GameObject Forkbeard;
        public GameObject FriendlyMuttonChops;
        public GameObject FuManchu;
        public GameObject GaribaldiBeard;
        public GameObject Goatee;
        public GameObject GoatPatch;
        public GameObject HandlebarMustache;
        public GameObject HorseshoeMustache;
        public GameObject HungarianMustache;
        public GameObject ImperialMustache;
        public GameObject MuttonChops;
        public GameObject NeckBeard;
        public GameObject NedKellyBeard;
        public GameObject PencilMustache;
        public GameObject PyramidMustache;
        public GameObject Shenandoah;
        public GameObject Sideburns;
        public GameObject SoulPatch;
        public GameObject TheZappa;
        public GameObject VanDykeBeard;
        public GameObject VerdiBeard;
    }

    [System.Serializable]
    public struct HairStylesPrefabs
    {
        public GameObject _Bald;
        public GameObject Afro;
        public GameObject BaldCrowned;
        public GameObject Bangs;
        public GameObject Beehive;
        public GameObject BigHair;
        public GameObject Blowout;
        public GameObject BobCut;
        public GameObject Bouffant;
        public GameObject BowlCut;
        public GameObject Bun;
        public GameObject Bunches;
        public GameObject ButchCut;
        public GameObject CeasarCut;
        public GameObject Chonmage;
        public GameObject CroydonFacelift;
        public GameObject CurtainedHair;
        public GameObject DoubleBuns;
        public GameObject DuckTail;
        public GameObject FeatheredHair;
        public GameObject FrenchBraid;
        public GameObject FringeBangs;
        public GameObject HalfUpdo;
        public GameObject HighAndTight;
        public GameObject HimeCut;
        public GameObject JheriCurl;
        public GameObject LibertySpikes;
        public GameObject Lob;
        public GameObject MarcelWaves;
        public GameObject Mohawk;
        public GameObject MopTop;
        public GameObject Mullet;
        public GameObject PIxieCut;
        public GameObject Pompadour;
        public GameObject PonyTail;
        public GameObject ShagCut;
        public GameObject Tonsure;
        public GameObject TopFade;
    }

    public BeardsPrefabs beards;
    public HairStylesPrefabs hairStyles;

    public GameObject GetBeards(Beards val)
    {
        switch (val)
        {
            default:
            case Beards._CleanShaven: return beards._CleanShaven;
            case Beards.AnchorBeard: return beards.AnchorBeard;
            case Beards.Balbo: return beards.Balbo;
            case Beards.ChinCurtain: return beards.ChinCurtain;
            case Beards.ChinstrapBeard: return beards.ChinstrapBeard;
            case Beards.CircleBeard: return beards.CircleBeard;
            case Beards.DaliMustache: return beards.DaliMustache;
            case Beards.DucktailBeard: return beards.DucktailBeard;
            case Beards.EnglishMustache: return beards.EnglishMustache;
            case Beards.Forkbeard: return beards.Forkbeard;
            case Beards.FriendlyMuttonChops: return beards.FriendlyMuttonChops;
            case Beards.FuManchu: return beards.FuManchu;
            case Beards.GaribaldiBeard: return beards.GaribaldiBeard;
            case Beards.Goatee: return beards.Goatee;
            case Beards.GoatPatch: return beards.GoatPatch;
            case Beards.HandlebarMustache: return beards.HandlebarMustache;
            case Beards.HorseshoeMustache: return beards.HorseshoeMustache;
            case Beards.HungarianMustache: return beards.HungarianMustache;
            case Beards.ImperialMustache: return beards.ImperialMustache;
            case Beards.MuttonChops: return beards.MuttonChops;
            case Beards.NeckBeard: return beards.NeckBeard;
            case Beards.NedKellyBeard: return beards.NedKellyBeard;
            case Beards.PencilMustache: return beards.PencilMustache;
            case Beards.PyramidMustache: return beards.PyramidMustache;
            case Beards.Shenandoah: return beards.Shenandoah;
            case Beards.Sideburns: return beards.Sideburns;
            case Beards.SoulPatch: return beards.SoulPatch;
            case Beards.TheZappa: return beards.TheZappa;
            case Beards.VanDykeBeard: return beards.VanDykeBeard;
            case Beards.VerdiBeard: return beards.VerdiBeard;
        }
    }

    public GameObject GetHairStyles(HairStyles val)
    {
        switch (val)
        {
            default:
            case HairStyles._Bald: return hairStyles._Bald;
            case HairStyles.Afro: return hairStyles.Afro;
            case HairStyles.BaldCrowned: return hairStyles.BaldCrowned;
            case HairStyles.Bangs: return hairStyles.Bangs;
            case HairStyles.Beehive: return hairStyles.Beehive;
            case HairStyles.BigHair: return hairStyles.BigHair;
            case HairStyles.Blowout: return hairStyles.Blowout;
            case HairStyles.BobCut: return hairStyles.BobCut;
            case HairStyles.Bouffant: return hairStyles.Bouffant;
            case HairStyles.BowlCut: return hairStyles.BowlCut;
            case HairStyles.Bun: return hairStyles.Bun;
            case HairStyles.Bunches: return hairStyles.Bunches;
            case HairStyles.ButchCut: return hairStyles.ButchCut;
            case HairStyles.CeasarCut: return hairStyles.CeasarCut;
            case HairStyles.Chonmage: return hairStyles.Chonmage;
            case HairStyles.CroydonFacelift: return hairStyles.CroydonFacelift;
            case HairStyles.CurtainedHair: return hairStyles.CurtainedHair;
            case HairStyles.DoubleBuns: return hairStyles.DoubleBuns;
            case HairStyles.DuckTail: return hairStyles.DuckTail;
            case HairStyles.FeatheredHair: return hairStyles.FeatheredHair;
            case HairStyles.FrenchBraid: return hairStyles.FrenchBraid;
            case HairStyles.FringeBangs: return hairStyles.FringeBangs;
            case HairStyles.HalfUpdo: return hairStyles.HalfUpdo;
            case HairStyles.HighAndTight: return hairStyles.HighAndTight;
            case HairStyles.HimeCut: return hairStyles.HimeCut;
            case HairStyles.JheriCurl: return hairStyles.JheriCurl;
            case HairStyles.LibertySpikes: return hairStyles.LibertySpikes;
            case HairStyles.Lob: return hairStyles.Lob;
            case HairStyles.MarcelWaves: return hairStyles.MarcelWaves;
            case HairStyles.Mohawk: return hairStyles.Mohawk;
            case HairStyles.MopTop: return hairStyles.MopTop;
            case HairStyles.Mullet: return hairStyles.Mullet;
            case HairStyles.PIxieCut: return hairStyles.PIxieCut;
            case HairStyles.Pompadour: return hairStyles.Pompadour;
            case HairStyles.PonyTail: return hairStyles.PonyTail;
            case HairStyles.ShagCut: return hairStyles.ShagCut;
            case HairStyles.Tonsure: return hairStyles.Tonsure;
            case HairStyles.TopFade: return hairStyles.TopFade;
        }
    }


}
