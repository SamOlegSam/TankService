using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TankService.Models;
using System.Web;
using Newtonsoft.Json;
using System.Timers;

namespace TankService
{   
    public partial class Service1 : ServiceBase   
   {
        public static Timer _timer;
        protected override void OnStart(string[] args)
        {  
            _timer.Start();
        }
                
        public static asopnEntities db1 = new asopnEntities();
        public static ChemLabEntities db2 = new ChemLabEntities();
        public class TankInventt
        {
            public DateTime Data { get; set; }
            public int Filial { get; set; }
            public int Rezer { get; set; }
            public int? Urov { get; set; }
            public int? UrovH2O { get; set; }
            public int? UrovNeft { get; set; }
            public double V { get; set; }
            public double VH2O { get; set; }
            public double VNeft { get; set; }
            public double Temp { get; set; }
            public double P { get; set; }
            public double MassaBrutto { get; set; }
            public double H2O { get; set; }
            public double Salt { get; set; }
            public double Meh { get; set; }
            public double BalProc { get; set; }
            public double BalTonn { get; set; }
            public double MassaNetto { get; set; }
            public double Hmin;
            public double Vmin;
            public double dMBalmin;
            public double dMNettomin;
            public double MBalTeh { get; set; }
            public double MNettoTeh { get; set; }
            public int type { get; set; }
        }
        //--------------------------------------------------------------
        public class tankRadar
        {
            public string id { get; set; }
            public bool s { get; set; }
            public string r { get; set; }
            public decimal v { get; set; }
            public decimal t { get; set; }
        }
        public class tankRadarObert
        {
            public List<tankRadar> ReadResults { get; set; }
        }
        //Создадим класс для заполнения данных уровней  и темпреатур резервуаров
        public class tankdatas
        {
            public int ID { get; set; }
            public DateTime Dat { get; set; }
            public int tankid { get; set; }
            public int FilialID { get; set; }
            public decimal level { get; set; }
            public decimal temp { get; set; }
        }
        
        public class CalculatorData
        {
            /// <summary>
            /// Oil density at a temperature of 15 °C.
            /// </summary>
            public double Dens15 { get; set; }

            /// <summary>
            /// Oil density at a temperature of 20 °C.
            /// </summary>
            public double Dens20 { get; set; }

            /// <summary>
            /// Values of the coefficient K20 / K15.
            /// </summary>
            public double K20_K15 { get; set; }

            /// <summary>
            /// Values of oil expansion coefficients.
            /// </summary>
            public double Betta { get; set; }

            /// <summary>
            /// Value of oil compressibility ratios.
            /// </summary>
            public double Gamma { get; set; }

            /// <summary>
            /// Adduction density.
            /// </summary>
            public double DensTP { get; set; }
        }

        public class Density636 : ICloneable
        {
            public double DensAreom { get; set; }
            public double TempAreom { get; set; }
            public double GradAreom { get; set; }
            public double TempReal { get; set; }
            public double Pressure { get; set; }

            public Density636()
            {
                DensAreom = 0;
                TempAreom = 0;
                GradAreom = 20;
                TempReal = 0;
                Pressure = 0;
            }

            public object Clone()
            {
                return new Density636
                {
                    DensAreom = this.DensAreom,
                    TempAreom = this.TempAreom,
                    GradAreom = this.GradAreom,
                    TempReal = this.TempReal,
                    Pressure = this.Pressure
                };
            }

        }
        //*******************************************************************************************************************
        public class Asopn
        {
            public static List<LastTanksResultMoz> LM = new List<LastTanksResultMoz>();  //список записей из представления химанализа по Мозырю
            public static List<LastTanksResultPol> LP = new List<LastTanksResultPol>();  //список записей из представления по Полоцку
            public static List<LastMechanResultMoz> LMM = new List<LastMechanResultMoz>(); //список записей механических примесей в резервуарах ЛПДС Мозырь

            public static LastTanksResultMoz LastM = new LastTanksResultMoz();  //экземпляр класса химанализ Мозыря
            public static LastTanksResultPol LastP = new LastTanksResultPol();  //экземпляр класса химанализ Полоцка
            public static LastMechanResultMoz LastMM = new LastMechanResultMoz(); //экземпляр класса механических примесей Мозыря


            public static List<TankInventt> TankI = new List<TankInventt>();

            public static trl_tank trl = new trl_tank();

            static List<calibration> calibrationList = new List<calibration>();

            static List<TankInv> ListInv = new List<TankInv>();
            static List<DonnyOstat> ListLastInv = new List<DonnyOstat>();

            public class Person
            {
                public string name { get; set; }
                public int age { get; set; }
            }

            public async static Task<List<TankInventt>> Spis()
            {
                //*******************************************************************************************
                //Заполняем список тегов для Мозыря

                List<TagCorrespondence> tagcorrespond = new List<TagCorrespondence>();
                tagcorrespond = db1.TagCorrespondence.Where(tu => tu.FilialID == 1 && tu.Enable == 1).ToList();

                List<string> listdata = new List<string>();
                foreach (var item in tagcorrespond)
                {
                    listdata.Add(item.lev);
                    listdata.Add(item.temp);
                }
                //********************************************************************************************

                var json1 = JsonConvert.SerializeObject(listdata);
                var dat = new StringContent(json1, Encoding.UTF8, "application/json");

                string url = "http://192.168.109.62:20008/Gateway/Read";
                var client = new HttpClient();
                var respons = await client.PostAsync(url, dat);

                string resul = await respons.Content.ReadAsStringAsync();
                tankRadarObert TankData = JsonConvert.DeserializeObject<tankRadarObert>(resul);

                //*******************************************************************************************
                //Заполняем список тегов для Полоцка

                List<TagCorrespondence> tagcorrespondPolock = new List<TagCorrespondence>();
                tagcorrespondPolock = db1.TagCorrespondence.Where(tu => tu.FilialID == 2 && tu.Enable == 1).ToList();

                List<string> listdataPolock = new List<string>();
                foreach (var item in tagcorrespondPolock)
                {
                    listdataPolock.Add(item.lev);
                    listdataPolock.Add(item.temp);
                }

                var jsonPolock = JsonConvert.SerializeObject(listdataPolock);
                var datPolock = new StringContent(jsonPolock, Encoding.UTF8, "application/json");

                string urlPolock = "http://192.168.155.136:20006/Gateway/Read";
                var clientPolock = new HttpClient();
                var responsPolock = await client.PostAsync(urlPolock, datPolock);

                string resulPolock = await responsPolock.Content.ReadAsStringAsync();
                tankRadarObert TankDataPolock = JsonConvert.DeserializeObject<tankRadarObert>(resulPolock);

                //*******************************************************************************************

                //Заполним список данными уровней и температур
                List<tankdatas> listtankdatas = new List<tankdatas>();
                foreach (var ty in tagcorrespond)
                {
                    tankdatas tankdatasITEM = new tankdatas();
                    tankdatasITEM.FilialID = ty.FilialID;
                    tankdatasITEM.tankid = ty.TankID;
                    tankdatasITEM.level = TankData.ReadResults.FirstOrDefault(t => t.id == ty.lev).v;
                    tankdatasITEM.temp = TankData.ReadResults.FirstOrDefault(t => t.id == ty.temp).v;
                    tankdatasITEM.Dat = DateTime.Now;
                    listtankdatas.Add(tankdatasITEM);
                }
                foreach (var tPol in tagcorrespondPolock)
                {
                    tankdatas tankdatasITEM = new tankdatas();
                    tankdatasITEM.FilialID = tPol.FilialID;
                    tankdatasITEM.tankid = tPol.TankID;
                    tankdatasITEM.level = TankDataPolock.ReadResults.FirstOrDefault(t => t.id == tPol.lev).v;
                    tankdatasITEM.temp = TankDataPolock.ReadResults.FirstOrDefault(t => t.id == tPol.temp).v;
                    tankdatasITEM.Dat = DateTime.Now;
                    listtankdatas.Add(tankdatasITEM);
                }
                //*******************************************************************************************

                LP = db2.LastTanksResultPol.ToList();
                LM = db2.LastTanksResultMoz.ToList();
                LMM = db2.LastMechanResultMoz.ToList();

                //Новый алгоритм
                ListLastInv = db1.DonnyOstat.ToList();
                //---------------------------------------------------------------------

                //Создадим экземпляр класса и список параметров для корректировки автоматически создаваемой инвентаризации
                Correct corr = new Correct();
                List<Correct> corrList = new List<Correct>();
                corrList = db1.Correct.ToList();


                calibrationList = db1.calibration.OrderBy(f => f.tankid).ThenBy(h => h.oillevel).ToList();
                decimal? UrovH2O;
                double UrovNeft;
                double V;
                double VH2O;
                double VNeft;
                double Umin;
                double Umax;
                double Vmin;
                double Vmax;
                double Upercent;
                double UminH2O;
                double UmaxH2O;
                double VminH2O;
                double VmaxH2O;
                double UpercentH2O;
                double Temp1;
                double P;
                double Psalt;
                double MassaBrutto;
                double H2O;
                double Salt;
                double Meh;
                double BalProc;
                double BalTonn;
                double MassaNetto;
                double Hmin;
                double MVmin;
                double MMin;
                double MNmin;
                int type;
                decimal? lev = 0;
                double VRasch;
                double VH2ORasch;
                double Kst;
                //foreach (var item in tankinfos)
                TankI.Clear();
                foreach (var item in listtankdatas)
                {
                    //Проверяем тип резервуара (ЖБР или РВС)
                    var GGG = db1.trl_tank.FirstOrDefault(r => r.TankID == item.tankid && r.FilialID == item.FilialID).trl_tanktype.IsSteel;
                    if (db1.trl_tank.FirstOrDefault(r => r.TankID == item.tankid && r.FilialID == item.FilialID).trl_tanktype.IsSteel == 0)
                    {
                        Kst = 0.00001;
                    }
                    else
                    {
                        Kst = 0.0000125;
                    }

                    decimal? temper;
                    if (ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid) == null)
                    {
                        UrovH2O = 0;
                    }
                    else
                    {
                        UrovH2O = ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O;
                    }
                    //Вывод общего объема нефти и подтоварной воды согласно общего уровня-------------------------------------------------------------
                    if (db1.trl_tank.FirstOrDefault(g => g.TankID == item.tankid & g.FilialID == item.FilialID) == null)
                    {
                        type = 77;
                    }
                    else
                    {
                        type = db1.trl_tank.FirstOrDefault(g => g.TankID == item.tankid & g.FilialID == item.FilialID).TypeID;
                    }

                    if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.filialid == item.FilialID) == null || calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.filialid == item.FilialID) == null)
                    {
                        V = 0;
                        Temp1 = 0;
                        VRasch = 0;
                    }
                    else
                    {
                        if (corrList.FirstOrDefault(g => g.filial == item.FilialID & g.tankid == item.tankid) == null)
                        {
                            lev = item.level;
                            temper = Convert.ToDecimal(item.temp);
                            //------если корректирующая таблица пустая, берем данные из танкрадара---------------------------------------------------------------
                            if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID) == null)
                            {
                                Umin = 0;
                            }
                            else
                            {
                                Umin = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID).oillevel;
                            }
                            Umax = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > lev & j.filialid == item.FilialID).oillevel;
                            if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID) == null)
                            {
                                Vmin = 0;
                            }
                            else
                            {
                                Vmin = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID).oilvolume;
                            }

                            Vmax = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > lev & j.filialid == item.FilialID).oilvolume;
                            Upercent = (Convert.ToDouble(item.level) - Umin) / (Umax - Umin);

                            V = Vmin + (Vmax - Vmin) * Upercent;
                            Temp1 = Convert.ToDouble(temper);

                            //Рассчитываем общий объем (нефть и подтоварная вода) по формуле
                            VRasch = V * (1 + 2 * Kst * (Temp1 - 20));

                        }
                        //---если запись в корректирующей таблице есть-----------------------------------------------------------------------------------------
                        else
                        {
                            if (corrList.FirstOrDefault(g => g.filial == item.FilialID & g.tankid == item.tankid).Uroven == null)
                            {
                                lev = Convert.ToDecimal(item.level);
                            }
                            else
                            {
                                lev = corrList.FirstOrDefault(g => g.filial == item.FilialID & g.tankid == item.tankid).Uroven;
                            }
                            if (corrList.FirstOrDefault(g => g.filial == item.FilialID & g.tankid == item.tankid).Temp == null)
                            {
                                temper = Convert.ToDecimal(item.temp);
                            }
                            else
                            {
                                temper = corrList.FirstOrDefault(g => g.filial == item.FilialID & g.tankid == item.tankid).Temp;
                            }

                            //---------------------------------------------------------------
                            if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID) == null)
                            {
                                Umin = 0;
                            }
                            else
                            {
                                Umin = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID).oillevel;
                            }
                            Umax = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > lev & j.filialid == item.FilialID).oillevel;
                            if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID) == null)
                            {
                                Vmin = 0;
                            }
                            else
                            {
                                Vmin = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= lev & g.filialid == item.FilialID).oilvolume;
                            }

                            Vmax = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > lev & j.filialid == item.FilialID).oilvolume;
                            Upercent = (Convert.ToDouble(lev) - Umin) / (Umax - Umin);

                            V = Vmin + (Vmax - Vmin) * Upercent;
                            Temp1 = Convert.ToDouble(temper);

                            //Рассчитываем общий объем (нефть и подтоварная вода) по формуле
                            VRasch = V * (1 + 2 * Kst * (Temp1 - 20));
                            //----------------------------------------------------------------
                        }
                    }
                    //-------Вывод объема подтоварной воды--------------------------------------------------------------------------------------------------------
                    //
                    if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.filialid == item.FilialID) == null || calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.filialid == item.FilialID) == null || ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid) == null || ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O == 0)
                    {
                        VH2O = 0;
                        VH2ORasch = 0;
                    }
                    else
                    {
                        if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O & g.filialid == item.FilialID) == null)
                        {
                            UminH2O = 0;
                        }
                        else
                        {
                            UminH2O = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O & g.filialid == item.FilialID).oillevel;
                        }
                        UmaxH2O = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O & j.filialid == item.FilialID).oillevel;
                        if (calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O & g.filialid == item.FilialID) == null)
                        {
                            VminH2O = 0;
                        }
                        else
                        {
                            VminH2O = calibrationList.LastOrDefault(g => g.tankid == item.tankid & g.oillevel <= ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O & g.filialid == item.FilialID).oilvolume;
                        }

                        VmaxH2O = calibrationList.FirstOrDefault(j => j.tankid == item.tankid & j.oillevel > ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O & j.filialid == item.FilialID).oilvolume;
                        UpercentH2O = (Convert.ToDouble(ListLastInv.FirstOrDefault(d => d.Filial == item.FilialID & d.TankID == item.tankid).UrovH2O) - UminH2O) / (UmaxH2O - UminH2O);
                        VH2O = VminH2O + (VmaxH2O - VminH2O) * UpercentH2O;

                        //Рассчитываем объем подтоварной воды по формуле
                        VH2ORasch = VH2O * (1 + 2 * Kst * (Temp1 - 20));
                    }
                    //---------------------------------------------------------------------------------------------------------------
                    //Вывод плотности нефти при температуре 20----------------------------------------------------------------------------
                    //Temp = 0;
                    P = 0;
                    Psalt = 0;
                    H2O = 0;
                    Salt = 0;
                    Meh = 0;
                    // VNeft = V - VH2O; Закоментировал объем по калибровочной таблице
                    //Рассчитываем объем нефти по формуле.
                    VNeft = VRasch - VH2ORasch;

                    if (item.FilialID == 1)
                    {
                        foreach (var i in LM.Where(f => f.tankid == item.tankid))
                        {
                            P = i.dens20.Value;
                            Psalt = i.densreal.Value;
                            if (i.water == null)
                            {
                                H2O = 0;
                            }
                            else
                            {
                                H2O = i.water.Value;
                            }
                            if (i.saltmg == null)
                            {
                                Salt = 0;
                            }
                            else
                            {
                                Salt = i.saltmg.Value;
                            }
                            // записываем значение механических примесей резервуара ЛПДС Мозырь
                            if (LMM.FirstOrDefault(g => g.tankid == item.tankid) == null)
                            {
                                Meh = 0;
                            }
                            else
                            {
                                Meh = LMM.FirstOrDefault(g => g.tankid == item.tankid).mechan.Value;
                            }
                        }
                    }
                    else
                    {
                        foreach (var i in LP.Where(f => f.tankid == item.tankid))
                        {

                            P = i.dens20.Value;
                            if (i.water == null)
                            {
                                H2O = 0;
                            }
                            else
                            {
                                H2O = i.water.Value;
                            }
                            if (i.saltmg == null)
                            {
                                Salt = 0;
                            }
                            else
                            {
                                Salt = i.saltmg.Value;
                            }
                            if (i.mechan == null)
                            {
                                Meh = 0;
                            }
                            else
                            {
                                Meh = i.mechan.Value;
                            }

                            Psalt = i.densreal.Value;
                        }
                    }

                    //-----------------------------------------------------------
                    var dens636 = new Density636()
                    {
                        DensAreom = P,
                        TempAreom = 20,
                        GradAreom = 20,
                        TempReal = Temp1,
                        Pressure = 0
                    };
                    var json = JsonConvert.SerializeObject(dens636);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    string urlDensCalc = "http://192.168.150.144:83/denscalc/get";
                    var clientDensCalc = new HttpClient();


                    var response = await clientDensCalc.PostAsync(urlDensCalc, data);

                    string result = response.Content.ReadAsStringAsync().Result;
                    var calculatorData = JsonConvert.DeserializeObject<CalculatorData>(result);
                    double PCalc;
                    if (calculatorData == null)
                    {
                        PCalc = 0;
                    }
                    else
                    {
                        PCalc = calculatorData.DensTP;
                    }
                    double SaltProc;
                    if (PCalc == 0)
                    {
                        SaltProc = 0;
                    }
                    else
                    {
                        SaltProc = Salt / Psalt / 10;
                    }
                    //-----------------------------------------------------------

                    MassaBrutto = PCalc * VNeft / 1000;
                    BalProc = H2O + SaltProc + +Meh;
                    BalTonn = MassaBrutto * BalProc / 100;
                    MassaNetto = MassaBrutto - BalTonn;
                    if (db1.trl_tank.FirstOrDefault(f => f.FilialID == item.FilialID & f.TankID == item.tankid) == null)
                    {
                        Hmin = 0;
                    }
                    else
                    {
                        Hmin = Convert.ToDouble(db1.trl_tank.FirstOrDefault(f => f.FilialID == item.FilialID & f.TankID == item.tankid).MinDopLevel);
                    }
                    if (db1.trl_tank.FirstOrDefault(f => f.FilialID == item.FilialID & f.TankID == item.tankid) == null)
                    {
                        MVmin = 0;
                    }
                    else
                    {
                        MVmin = Convert.ToDouble(db1.trl_tank.FirstOrDefault(f => f.FilialID == item.FilialID & f.TankID == item.tankid).MinDopVol);
                    }

                    MMin = PCalc * MVmin / 1000;
                    MNmin = MMin - (MMin * BalProc / 100);
                    //MNmin = MMin * (100 - BalProc) / 100;
                    //VH2O = 0; //Задаем значение объема подтоварной воды = 0, т.к. уровень подтоварной воды нигде не указывается
                    UrovNeft = Convert.ToDouble(lev) - Convert.ToDouble(UrovH2O);
                    // VNeft = V - VH2O;
                    //---------------------------------------------------------------------------------------------------------------
                    //Преобразуем дату (сделаем время 00:00)
                    string dt;
                    if (item.Dat.Hour>=10 && item.Dat.Hour <=23)
                    {
                      dt = Convert.ToString(item.Dat).Substring(0, 14) + "00:00.000";
                    }                    
                    else
                    {
                     dt = Convert.ToString(item.Dat).Substring(0, 13) + "00:00.000";
                    }
                    DateTime dtTime = Convert.ToDateTime(dt);
                    //--------------------------------------
                    TankI.Add(new TankInventt() { Data = dtTime, Filial = item.FilialID, Rezer = item.tankid, Urov = Convert.ToInt32(lev), UrovH2O = Convert.ToInt32(UrovH2O), UrovNeft = Convert.ToInt32(UrovNeft), V = V, P = PCalc, Temp = Temp1, MassaBrutto = MassaBrutto, H2O = H2O, Salt = SaltProc, Meh = Meh, BalProc = BalProc, BalTonn = BalTonn, MassaNetto = MassaNetto, Hmin = Hmin, Vmin = MVmin, dMBalmin = MMin, dMNettomin = MNmin, type = type, VH2O = VH2ORasch, VNeft = VNeft }); ;
                }
                return TankI;
            }
        }
        //*******************************************************************************************************************
        //Сервис в данной службе не используем
        public Service1()
        {
            InitializeComponent();
            _timer = new Timer(2000); 
            _timer.Elapsed += Count;
        }
        //*****************************************************************************************
        static async void Count(object sender, ElapsedEventArgs e)
        {
            if((DateTime.Now).Minute  == 00)                
            {               
                //********************************************************************************************************************

            _timer.Stop();// Останавливаем таймер на время записи инвентаризации и проверки времени для записи суточной сводки
                                
                TankInv TankinV = new TankInv();
                //Создадим список TankInv для последующей передачи в колбек для записи в БД
                //**********************************************************************************************************************************
                try
            {
                File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "-----------ИНВЕНТАРИЗАЦИЯ НА " + DateTime.Now.ToString() + "\n");
                File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "-----------------------------------------------------------------\n");
                foreach (var item in await Asopn.Spis())
                {
                    //---Запись таблицы в БД---------------------------------------------------------------------

                    TankinV.Data = item.Data;
                    TankinV.Filial = item.Filial;
                    TankinV.Rezer = item.Rezer;
                    TankinV.Urov = item.Urov;
                    TankinV.UrovH2O = item.UrovH2O;
                    TankinV.UrovNeft = item.UrovNeft;
                    TankinV.V = Convert.ToDecimal(Math.Round(item.V, 3));
                    TankinV.Temp = Convert.ToDecimal(Math.Round((double)item.Temp, 2));
                    TankinV.P = Convert.ToDecimal(Math.Round((double)item.P, 2));
                    TankinV.MassaBrutto = Convert.ToDecimal(Math.Round((double)item.MassaBrutto, 3));
                    TankinV.H2O = Convert.ToDecimal(Math.Round((double)item.H2O, 4));
                    TankinV.Salt = Math.Round(Convert.ToDecimal(item.Salt), 4);
                    TankinV.Meh = Convert.ToDecimal(Math.Round((double)item.Meh, 4));
                    TankinV.BalProc = Math.Round(Convert.ToDecimal(item.BalProc), 4);
                    TankinV.BalTonn = Convert.ToDecimal(item.BalTonn);
                    TankinV.MassaNetto = Convert.ToDecimal(Math.Round((double)item.MassaNetto, 1));
                    TankinV.HMim = Convert.ToDecimal(Math.Round((double)item.Hmin, 0));
                    TankinV.VMin = Convert.ToDecimal(Math.Round((double)item.Vmin, 1));
                    TankinV.MBalMin = Convert.ToDecimal(Math.Round((double)item.dMBalmin, 1));
                    TankinV.MNettoMin = Convert.ToDecimal(Math.Round((double)item.dMNettomin, 1));
                    TankinV.type = item.type;
                    TankinV.VH2O = Convert.ToDecimal(item.VH2O);
                    TankinV.VNeft = Convert.ToDecimal(item.VNeft);

                        db1.TankInv.Add(TankinV);
                        db1.SaveChanges();
                        File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", item.Data + " | " + item.Filial + " | " + item.Rezer + " | " + item.Urov + " | " + item.UrovH2O + " | " + item.UrovNeft + " | " + item.V + " | " + item.Temp + " | " + item.P + " | " + item.Salt + " | " + item.MassaBrutto + " | " + item.H2O + " | " + item.Salt + " | " + "\n");
                }
                //------------------------------------------------------
                File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "Запись инвентаризации успешно добавлена в БД " + DateTime.Now + "\n");
                File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "-----------------------------------------------------------------\n");
                //------------------------------------------------------------
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "Ошибка записи! Код ошибки: " + ex.ToString() + "\n");
            }

            //***************************************************************************************************************************************************************

            //Проверяем если время равно 00:00 тогда запишем данные в таблицу для суточной сводки            

            if (DateTime.Now.Hour == 00)            
            {
            List<TankInv> TableInv = new List<TankInv>();
            TableInv = db1.TankInv.OrderByDescending(o => o.Data).ToList();
            DateTime LastDate = TableInv.FirstOrDefault().Data;
                try
                {
                    {
                        SvodkaSutki SutSv = new SvodkaSutki();
                        var TabInv = TableInv.Where(y => y.Data == LastDate).GroupBy(k => k.Filial);
                        File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "СУТОЧНАЯ СВОДКА НА " + DateTime.Now.ToString() + "\n");
                        foreach (var t in TabInv)
                        {
                            int kol = 0;
                            decimal? VNeftItog = 0;
                            decimal? MassaBalItog = 0;
                            decimal? BalTonnItog = 0;
                            decimal? MassaNettoItog = 0;
                            decimal? MassaBalMinItog = 0;
                            decimal? MassaNettoMinItog = 0;
                            decimal? MassaBruttoOstat = 0;

                            foreach (var i in t)
                            {
                                kol++;
                                VNeftItog = VNeftItog + i.VNeft;
                                MassaBalItog = MassaBalItog + i.MassaBrutto;
                                BalTonnItog = BalTonnItog + i.BalTonn;
                                MassaNettoItog = MassaNettoItog + i.MassaNetto;
                                MassaBalMinItog = MassaBalMinItog + i.MBalMin;
                                MassaNettoMinItog = MassaNettoMinItog + i.MNettoMin;
                                MassaBruttoOstat = MassaBruttoOstat + i.MBalMin;
                            }
                            SutSv.FilialID = t.Key;
                            SutSv.Dat = LastDate;
                            SutSv.VNeftItog = VNeftItog;
                            SutSv.MassaBalItog = MassaBalItog;
                            SutSv.MassaNettoItog = MassaNettoItog;
                            SutSv.MassaBruttoOstat = MassaBruttoOstat;
                            SutSv.MassaNettoMinItog = MassaNettoMinItog;
                            SutSv.MassaItog = MassaNettoItog - MassaNettoMinItog;
                            SutSv.DatWrite = DateTime.Now;

                            db1.SvodkaSutki.Add(SutSv);
                            db1.SaveChanges();

                            File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", t.Key + " | " + LastDate + " | " + VNeftItog + " | " + MassaBalItog + " | " + MassaNettoItog + " | " + MassaBruttoOstat + " | " + MassaBruttoOstat + " | " + MassaNettoMinItog + " | " + (MassaNettoItog - MassaNettoMinItog) + " | " + "\n");
                        }
                        File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "Данные суточной сводки записаны " + DateTime.Now + "\n");
                        File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "----------------------------------------------------\n");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(@"C:/Project_Planer/tankInventService/LogTankInvent.txt", "Ошибка записи! Код ошибки: " + ex.ToString() + "\n");
                }
            }
            else
            {
                
            }
               await Task.Delay(60000); //Ожядаем 1 минуту после записи инвентаризации, чтобы инвентаризация не записалась второй раз, т.к таймер запускается каждые 2 секунды и может записываться инвентаризация пока не пройдет минута
               _timer.Start();
        }
            else
            {
                
            }
        }      
        protected override void OnStop()
        {

        }
    }
}
