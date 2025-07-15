using GetStartedApp.AutoMapper;
using GetStartedApp.Core.Helpers;
using GetStartedApp.Models;
using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Services;
using Prism.Dialogs;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.Product
{
    public class RecipeViewModel : ViewModelBase
    {

        private readonly IBase_Version_Primary_Config_Service _version_Primary_Config_Service;
        private readonly IBase_Version_Attribute_Config_Service _attribute_Config_Service;
        private readonly IProduct_Recipe_Service _recipe_Service;
        private readonly IDialogService _dialogService;
        private readonly IAppMapper _appMapper;

        public RecipeViewModel(IBase_Version_Primary_Config_Service version_Primary_Config_Service,
                                IBase_Version_Attribute_Config_Service attribute_Config_Service,
                                IProduct_Recipe_Service recipe_Service,
                                IDialogService dialogService,
                                IAppMapper appMapper)
        {
            _version_Primary_Config_Service = version_Primary_Config_Service;
            _attribute_Config_Service = attribute_Config_Service;
            _recipe_Service = recipe_Service;
            _dialogService = dialogService;
            _appMapper = appMapper;
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //初始化型号Tree
            InitVersionTree();
        }

        #region 数据
        private ObservableCollection<VersionPrimaryDto> _VersionTree;

        public ObservableCollection<VersionPrimaryDto> VersionTree
        {
            get { return _VersionTree; }
            set { SetProperty(ref _VersionTree, value); }
        }


        private List<RecipeDto> _Recipes = new List<RecipeDto>();//用来保存当前工单下OP10，OP20，OP30的配方
        private ObservableCollection<RecipeSTDto> _OP10Datas;

        public ObservableCollection<RecipeSTDto> OP10Datas
        {
            get { return _OP10Datas; }
            set { SetProperty(ref _OP10Datas, value); }
        }

        private ObservableCollection<RecipeSTDto> _OP20Datas;

        public ObservableCollection<RecipeSTDto> OP20Datas
        {
            get { return _OP20Datas; }
            set { SetProperty(ref _OP20Datas, value); }
        }

        private ObservableCollection<RecipeSTDto> _OP30Datas;

        public ObservableCollection<RecipeSTDto> OP30Datas
        {
            get { return _OP30Datas; }
            set { SetProperty(ref _OP30Datas, value); }
        }

        private VersionSecondDto _selectedItem;
        public VersionSecondDto selectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    if (value != null)
                    {
                        SelectedItemChanged(value);
                    }
                }
            }
        }


        #endregion

        #region 方法

        private void InitVersionTree()
        {
            VersionTree = _appMapper.Map<List<VersionPrimaryDto>>(_version_Primary_Config_Service.GetVersionPrimayTree()).ToObservableConllection();

        }
        private string _secondName = string.Empty;
        private int _secondId = 0;
        private void SelectedItemChanged(VersionSecondDto value)
        {
            List<int> secondIds = new List<int>();
            if (value != null)
            {
                secondIds.Add(value.Id);
                _secondName = value.Name;
                _secondId = value.Id;


                var recipes = _recipe_Service.GetRecipeBySecondId(value.Id);
                if (recipes.Count == 3)
                {
                    _Recipes = _appMapper.Map<List<RecipeDto>>(recipes);
                    //选择对应的OP
                    OP10Datas = new ObservableCollection<RecipeSTDto>();
                    OP20Datas = new ObservableCollection<RecipeSTDto>();
                    OP30Datas = new ObservableCollection<RecipeSTDto>();
                    foreach (var item in _Recipes[0].STs)
                    {
                        item.Parameters.RemoveAll(x => x.Name.Contains("参数"));
                        if (item.Parameters.Count <= 0)
                        {
                            continue;
                        }
                        if (item.Name == "ST04")
                        {
                            item.Parameters[0].Items = _DHGColor;
                        }
                        OP10Datas.Add(item);
                    }
                    foreach (var item in _Recipes[1].STs)
                    {
                        item.Parameters.RemoveAll(x => x.Name.Contains("参数"));
                        if (item.Parameters.Count <= 0)
                        {
                            continue;
                        }
                        OP20Datas.Add(item);
                    }
                    foreach (var item in _Recipes[2].STs)
                    {
                        item.Parameters.RemoveAll(x => x.Name.Contains("参数"));
                        if (item.Parameters.Count <= 0)
                        {
                            continue;
                        }
                        if (item.Name == "ST06")
                        {
                            item.Parameters[2].Items = _Sms;
                        }
                        else if (item.Name == "ST13")
                        {
                            item.Parameters[0].Items = _ST13LaserMode;
                        }
                        else if (item.Name == "ST15")
                        {
                            item.Parameters[0].Items = _ST15LaserMode;
                            //item.Parameters[1].Items = _ST15LaserIs180;
                        }
                        else if (item.Name == "ST05")
                        {
                            item.Parameters[0].Items = _Sms;
                        }
                        else if (item.Name == "ST19")
                        {
                            item.Parameters[0].Items = _ReelModel;
                        }
                        OP30Datas.Add(item);
                    }
                   // IsEnable = "Visible";
                }
                else
                {
                   
                }
            }
        }
        #endregion


        #region combox列表

        private readonly List<string> _DHGColor = new List<string>()
        {
            "未选择",
            "浅蓝色",
            "粉色",
            "黑色"
        };

        private readonly List<string> _ST13LaserMode = new List<string>()
        {
              "未选择",
            //  "内圈(有点)",
            //  "外圈",
             // "内圈(无点)",
             // "外圈(序列号分开)"

             "16AK2", //"16小弧",
             "16AK1" ,//"16大弧",
              "14AK2",//"14小弧",
              "14AK1"//"14大弧"
           
        };

        private readonly List<string> _ST15LaserMode = new List<string>()
        {
              "未选择",
              "固定码",
               "二维码",
              "二维码+固定码"
        };
        private readonly List<string> _ST15LaserIs180 = new List<string>()
        {
              "未选择",
              "不旋转",
              "旋转180°",
        };


        private readonly List<string> _ReelModel = new List<string>()
        {
              "未选择",
              "6008",
              "1027",
        };

        private readonly List<string> _Sms = new List<string>
        {
            "未选择",
            "1473369-1",
             "C25212B0022492",
            "C25212B0022422S",
             "C25214B002_1012",
             "69005117",
             "69005103",
             "69005120",
        };

        #endregion
    }
}
