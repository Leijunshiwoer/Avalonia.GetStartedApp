using SmartCommunicationForExcel.Implementation.Beckhoff;
using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Implementation.Omron;
using SmartCommunicationForExcel.Implementation.Siemens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Extend
{
    public static class Hellper
    {
        public static SiemensEventIO GetSiemensEventIOByTagName(this List<SiemensEventIO> siemensEventIOs, string tagName)
        {
            try
            {
                return siemensEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static OmronEventIO GetOmronEventIOByTagName(this List<OmronEventIO> omronEventIOs, string tagName)
        {
            try
            {
                return omronEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  static MitsubishiEventIO GetMitsubishiEventIOByTagName(this List <MitsubishiEventIO> mitsubushiEventIOs, string tagName)
        {
            try
            {
                return mitsubushiEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static BeckhoffEventIO GetBeckhoffEventIOByTagName(this List<BeckhoffEventIO> beckhoffEventIOs, string tagName)
        {
            try
            {
                return beckhoffEventIOs.Where(it => it.TagName == tagName).SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
