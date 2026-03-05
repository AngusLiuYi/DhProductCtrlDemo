#define HANDLETYPE_UINT64
//#define MACHINE_X86

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi
{
    /// <summary>
    /// 兼容早期版本使用UInt32 HANDLETYPE，后期版本系统使用UInt64 HANDLETYPE。
    /// </summary>
#if HANDLETYPE_UINT64
    using HANDLETYPE = UInt64;
#else
    using HANDLETYPE = UInt32;
#endif

    public class ImcApi
    {
    
#if MACHINE_X86
        public const string EtherCATConfigApiDllName = "IMC_API_x86.dll";
#else
        public const string EtherCATConfigApiDllName = "IMC_API_x64.dll";
#endif

        public const uint EXE_SUCCESS = 0x00000000;

        /******************板卡资源结构体定义***************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TRsouresNum
        {
            public Int16 terminalexist; // 端子板是否连接
            public Int16 axNum;         // ECAT 总线的所有轴数
            public Int16 diNum;         // ECAT 总线上DI 资源
            public Int16 doNum;         // ECAT 总线上DO 资源
            public Int16 adNum;         // ECAT 总线上AD 资源
            public Int16 daNum;         // ECAT 总线上DA 资源
            public Int16 encNum;        // ECAT 总线上ENC资源
        };

        /***************轴运行安全参数结构体定义************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TMtPara
        {
            public double bgVel;    // 起始速度值 pulse/ms
            public double maxVel;   // 最大速度 pulse/ms
            public double maxAcc;   // 最大加速度 pulse/ms^2
            public double maxDec;   // 最大减速度 pulse/ms^2
            public double maxJerk;  // 最大加加速度 pulse/ms^3
            public double stopDec;  // 平滑停止减速度 pulse/ms^2
            public double eStopDec; // 急停减速度 pulse/ms^2
        };

        /***************轴运行到位参数结构体定义************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TAxAttriPara
        {
            public Int16 arrivalBand; 		// 到位误差 pulse
            public Int16 arrivalTime; 		// 到位保持时间 ms
            public Int32 errorLmt; 		    // 最大跟随误差 pulse
            public Int32 softPosLimitPos; 	// 软正限位 pulse
            public Int32 softNegLimitPos; 	// 软负限位 pulse
        };

        /***************轴安全检查参数结构体定义************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TAxCheckEn
        {
            public Int16 alarmEn; 		// 报警是否有效标志
            public Int16 softLmtEn; 	// 软限位是否有效标志
            public Int16 hwLmtEn; 		// 硬限位是否有效标志
            public Int16 errorLmtEn; 	// 跟随误差是否检查标志
        };

        

        /******************采样参数结构体定义***************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TSamplePara
        {
            public Int16 interval;       // 采样时间间隔
            public Int16 trigType;       // 触发采样类型：0 立即 1 延时 2 本地di 3 ECAT di
            public Int16 delay;          // 延时时间 ms
            public Int16 diNo;           // di输入号
            public Int16 diLevel;        // di的触发输入值 0或1
        };
       
        /******************回原功能参数结构体定义***************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct THomingPara
        {
            public Int16 homeMethod;        // 回原点方法
            public Int32 offset;            // 回原点后的零点偏执 pulse
            public UInt32 highVel;          // 高速搜索减速点速度 pulse/ms
            public UInt32 lowVel;           // 搜索原点低 pulse/ms
            public UInt32 acc; 				// 加速度 pulse/ms^2
            public UInt32 overtime;         // 超时时间 ms
            public Int16 posSrc;            // 仅对端子板轴回零有效，ECAT轴无效（暂不生效）
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct THomingParaInUint
        {
            public Int16 homeMethod;        // 回原点方法
            public double offset;           // 回原点后的零点偏执 unit
            public double highVel;          // 高速搜索减速点速度 unit/s
            public double lowVel;           // 搜索原点低 unit/s
            public double acc;              // 加速度 unit/s^2
            public UInt32 overtime;         // 超时时间 ms
            public Int16 posSrc;            // 仅对端子板轴回零有效，ECAT轴无效（暂不生效）
        };

      
        /******************插补功能高级参数结构体定义***************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TCrdAdvParam
        {
            public Int16 userVelMode;          // 用户速度规划模式：0 系统前瞻速度规划，1 用户设定速度规划(默认：0)
            public Int16 transMode;            // 过渡模式 1：无过渡 2：圆弧过渡 （默认:2）
            public Int16 noDataProtect;        // 数据断流保护：0不保护，1保护 （默认：1）
            public Int16 circAccChangeEn;      // 圆弧变加速使能：0不变加速，1变加速 （默认：0） 
            public Int16 noCoplaneCircOptm;    // 异面圆弧优化：0不开启，1开启
            public double turnCoef;            // 拐弯系数: [0.01~50]（默认：1.0）
            public double tol;                 // 插补精度: 大于0的值（默认：0 ，单位取决于设置的当量）
        };

        /******************电子齿轮功能参数结构体定义***************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TGearParam
        {
            public double masterScale;		// 主轴齿数
            public double slaveScale;		// 从轴齿数
            public Int16 masterNo;			// 主轴轴号
            public Int16 masterType;		// 主轴类型
            public Int16 dirMode;			// 方向模式
            public Int32 masterSlopeDis;	// 主轴离合区
        };

        /******************事件功能参数结构体定义***************************/
        [StructLayout(LayoutKind.Sequential)]
        public struct TEventIO
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int16[] inType;          // 输入的类型：0表示物理输入 1表示虚拟输入
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int16[] inPortNo;		// 输入的端口号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int16[] inInvert;		// 输入是否取反：0表示不取反 1表示取反
            public Int16 inOperator;		// 输入操作符：0与操作 1或操作  2异或操作 3 单输入
            public Int16 outType;			// 输出类型：0表示物理输出 1表示虚拟输出（虚拟输出一般作为其他单元的输入源）
            public Int16 outPortNo;         // 输出端口
            public Int16 outInvert;         // 输出取反：0表示不取反 1表示取反
            public Int16 activeType;		// 生效的类型：0单次生效 1持续生效
            public Int16 triggerType;       // 触发类型：0:电平触发 1：上升沿触发 2：下降沿触发
            public Int16 delayTime;         // 延时输出的时间:ms
        };

        public struct TEventDiMotion
        {
            public Int16 inType;  				// 输入的类型：0表示物理输入 1表示虚拟输入
            public Int16 inPortNo;				// 输入的端口号
            public Int16 motionAxNo;			// 触发的运动轴号
            public Int16 delay;				    // 单位：ms
            public Int16 motionType;            // 启停类型 0:PTP启动  1：Jog启动  2：平滑停止  3：急停  4：更新参数
            public Int16 triggerType;           // 触发类型 0:电平触发（高电平） 1:上升沿触发 2：下降沿触发
            public Int16 invertBit;			    // 取反位，输入取反
            public double tgtPos;				// 目标位置
            public double tgtVel;				// 目标速度
            public double acc;					// 加速度
            public double dec;					// 减速度
        };

        public struct TEventCompareOut
        {
            public Int16 inType;  				// 输入类型：0编码器 1规划位置
            public Int16 portNo;				// 端口号
            public Int16 outPortNo;             // DO输出端口号
            public Int16 outVal;				// 输出电平值
            public Int16 outType;				// 输出类型：0电平 1脉宽 2虚拟值（虚拟输出一般作为其他单元的输入源）
            public Int16 cmpType;				// 比较的方式：0大于等于  1小于
            public Int16 pulseWidth;			// 脉冲宽度
            public double comparePos;			// 比较的位置
        };

        /*****************端子板多维位置比较参数结构体定义*************************/
        public struct TMultiCmpData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] compareData;
        };

        /*********************从站信息结构体定义****************************************/
        public struct TSlaveInfo
        {
            public UInt16 device_type;     // 从站类型
            public UInt32 vendor_id;       // 设备ID
            public UInt32 product_code;    // 产品代码
            public UInt32 revisionNo;      
            public UInt16 axis_num;        // 当前从站轴数
            public Int16 actStation;       // 当前从站对应的实际站号
            public UInt32 aliasNo;         // 别名
            public UInt16 opMode;          // 自由协议从站
        }

        /*********************主站信息结构体定义****************************************/
        public struct TMasterInfo
        {
            public UInt16 sysHwCfg;        
            public UInt32 cycleTime;
            public UInt16 alias_mode;
            public UInt16 stationCnt;
            public UInt16 svCnt;
            public UInt16 ioCnt;
            public UInt16 aioCnt;
            public UInt16 ptCnt;
            public UInt16 glCnt;
            public UInt32 pdolen;
        }

        /**********************ECAT板卡资源定义*********************************/
        public const uint MAX_CARD_NUM = (4);

        /**********************ECAT板卡主站状态定义*******************************
        ******************说明：以下枚举针对IMC_GetECATMasterSts函数**************/
        public const uint EC_MASTER_IDLE = (0);             // EtherCat尚未初始化
        public const uint EC_MASTER_INIT = (1);             // EtherCat初始化
        public const uint EC_MASTER_SCAN_SLAVE = (2);       // EtherCat正在扫描从站设备
        public const uint EC_MASTER_SCAN_SLAVE_END = (3);   // EtherCat扫描从站设备结束
        public const uint EC_MASTER_SCAN_MODULES = (4);     // EtherCat正在扫描从站设备MODULES
        public const uint EC_MASTER_SCAN_MODULES_END = (5); // EtherCat扫描从站设备MODULES结束
        public const uint EC_MASTER_OP = (6);               // EtherCat进入OP状态
        public const uint EC_MASTER_ERR = (7);              // EtherCat链路状态有错误

        /**********************ECAT板卡从站状态定义*******************************
        ******说明：以下枚举针对IMC_GetSlaveCurSts， IMC_GetSlaveReqSts函数*******/
        public const uint EC_SLAVE_STATE_UNKNOWN = (0x00);  // EtherCat从站在未知状态
        public const uint EC_SLAVE_STATE_INIT = (0x01);     // EtherCat从站在初始状态
        public const uint EC_SLAVE_STATE_PREOP = (0x02);    // EtherCat从站在PREOP状态
        public const uint EC_SLAVE_STATE_BOOT = (0x03);     // EtherCat从站在BOOT状态
        public const uint EC_SLAVE_STATE_SAFEOP = (0x04);   // EtherCat从站在SAVEOP状态
        public const uint EC_SLAVE_STATE_OP = (0x08);       // EtherCat从站在OP状态
        public const uint EC_SLAVE_STATE_ACK_ERR = (0x10);  // EtherCat从站有错误


        /**********************EcatErrorCode定义*********************************
        *******说明：以下宏定义针对IMC_GetEcatErrCode函数,其他为内部错误***********/
        public const uint ERROR_CODE_NO_MASTER = (0x0001);          // 不存在ECAT MASTER设备
        public const uint ERROR_CODE_INVALID_DOMAIN = (0x0002);     // 非法Domain域
        public const uint ERROR_CODE_NO_SUCH_SLAVE = (0x0003);      // 没有这个从站
        public const uint ERROR_CODE_INVALID_PDO = (0x0004);        // 非法PDO
        public const uint ERROR_CODE_INVALID_SDO = (0x0005);        // 非法SDO
        public const uint ERROR_CODE_INVALID_ENTRY = (0x0006);      // 非法ENTRY   
        public const uint ERROR_CODE_NOMEM = (0x0007);              // ECAT协议栈分配内存失败   
        public const uint ERROR_CODE_MASTER_FAIL = (0x0008);        // 启动MASTER主站失败  
        public const uint ERROR_CODE_COMMON_FAIL = (0x0009);        // 通信失败 
        public const uint ERROR_CODE_CYCCOM_FAIL = (0x000A);        // 周期定时器失败        
        public const uint ERROR_CODE_BUFFCFG_FAIL = (0x000B);       // PDO buffer配置失败      
        public const uint ERROR_CODE_INITMODULFAIL = (0x000C);      // 初始化Module失败      
        public const uint ERROR_CODE_PASECFGFAIL = (0x000D);        // 解析XML设备配置失败 
        public const uint ERROR_CODE_CFGECATDSPFAIL = (0x000E);     // 同步配置到DSP失败
        public const uint ERROR_CODE_DOMAINREGFAIL = (0x000F);      // Domain域配置信息有误
        public const uint ERROR_CODE_CREATIMERFAIL = (0x0010);      // 创建定时器失败
        public const uint ERROR_CODE_STARTIMERFAIL = (0x0011);      // 启动定时器失败
        public const uint ERROR_CODE_CFGVERSELFAIL = (0x0012);      // 获取版本信息失败
        public const uint ERROR_CODE_CFGMEMORYFAIL = (0x0013);      // 分配过程数据buffer失败
        public const uint ERROR_CODE_MEMWARNINFAIL = (0x0014);      // 创建共享数据失败
        public const uint ERROR_CODE_CFGREGISTFAIL = (0x0015);      // 配置reg失败
        public const uint ERROR_CODE_CFGDIFFONLINE = (0x0016);      // 在线从站与配置不一致
        public const uint ERROR_CODE_AXIS_NUM_BEYOND = (0x0017);    // 轴配置数量超过板卡最大支持轴数；
        public const uint ERROR_CODE_SLAVE_OFFLINE = (0x001a);      // 从站掉线错误，高8位为轴号，即bit[15:8]-轴号  
        public const uint ERROR_CODE_SDOBF_NONECAT = (0x001b);      // SDO缓冲区收到非ECAT帧错误
        public const uint ERROR_CODE_PORT0_NOTLINK = (0x001c);      // 端口未接ECAT设备错误
        public const uint ERROR_CODE_SETIR_STARTIM_ERR = (0x001d);  // 开始DC及中断工作发送错误
        public const uint ERROR_CODE_SET_CYCLETIME_PARA_ERR = (0x001e);     // 设置周期时间参数错误
        public const uint ERROR_CODE_COE_SDO_INIT_ERR = (0x001f);   // 在初始化阶段coe配置错误
        public const uint ERROR_CODE_SLAVE_STATE_ERR = (0x0020);    // 从站状态错误，高8位为轴号，即bit[15:8]-轴号    
        public const uint ERROR_CODE_WR_STACMD_ERR = (0x0021);      // 写写命令字失败
        public const uint ERROR_CODE_RDSTS_REG_ERR = (0x0022);      // 读状态字失败
        public const uint ERROR_CODE_RD_DLLERR_ERR = (0x0023);      // 读取DLL失败
        public const uint ERROR_CODE_RD_SDOBF_ERR = (0x0024);       // 读SDO缓冲区错误
        public const uint ERROR_CODE_RD_ERROR_CODE_SDOBF_LEN_ERR = (0x0025);     // 读SDO缓冲区长度错误
        public const uint ERROR_CODE_SDOBF_LEN_ERR = (0x0026);      // SDO长度错误
        public const uint ERROR_CODE_SDOBF_RCV_ERR = (0x0027);      // 接收SDO错误
        public const uint ERROR_CODE_SDOBF_BUSY_ERR = (0x0028);     // SDO操作忙状态
        public const uint ERROR_CODE_SDOBF_DATAGRAM = (0x0029);     // SDO数据帧错误
        public const uint ERROR_CODE_RD_PDOBF_ERR = (0x002a);       // 读取PDO缓冲区错误
        public const uint ERROR_CODE_RD_ERROR_CODE_PDOBF_LEN_ERR = (0x002b);     // 读取PDO缓冲区长度错误
        public const uint ERROR_CODE_PDOBF_LEN_ERR = (0x002c);      // PDO长度错误
        public const uint ERROR_CODE_PDOBF_RCV_ERR = (0x002d);      // PDO接收错误
        public const uint ERROR_CODE_NETCARDOPEN_ERR = (0x002e);    // 打开网卡失败
        public const uint ERROR_CODE_GPMC_IOCTRL_ERR = (0x002f);    // 打开GPMC失败
        public const uint ERROR_CODE_GPMC_ECATRD_ERR = (0x0030);    // GPMC读ECAT失败
        public const uint ERROR_CODE_GPMC_ECATWR_ERR = (0x0031);    // GPMC写ECAT失败
        public const uint ERROR_CODE_RD_TXTMSTMP_ERR = (0x0032);    // GPMC读取ECAT发送时间失败
        public const uint ERROR_CODE_RD_RXTMSTMP_ERR = (0x0033);    // GPMC读取ECAT接收时间失败
        public const uint ERROR_CODE_RD_PDOBFLFT_ERR = (0x0034);    // GPMC读取ECAT PDO缓冲区剩余失败
        public const uint ERROR_CODE_RD_APPTIME_ERR = (0x0035);     // GPMC读取ECAT APP时间失败
        public const uint ERROR_CODE_WR_BUFF_ERR = (0x0036);        // 写缓冲区错误 
        public const uint ERROR_CODE_SLAVE_SII_ERR = (0x0037);      // E2ROM 信息有误,处理方法：一般为从站保存的e2rom信息有误，可以使用twincat确认并联系厂家。

        /***********************AbortCode定义********************************
        *******说明：以下宏定义针对IIMC_GetEcatSdo\IMC_SetEcatSdo函数*********/
        public const uint ABORT_CODE1 = (0x05030000);       // 从站Toggle bit 没有变化
        public const uint ABORT_CODE2 = (0x05040000);       // SDO 访问超时
        public const uint ABORT_CODE3 = (0x05040001);       // 客户端/服务器 命令非法或未知
        public const uint ABORT_CODE4 = (0x05040005);       // 内存溢出
        public const uint ABORT_CODE5 = (0x06010000);       // 不支持访问该对象
        public const uint ABORT_CODE6 = (0x06010001);       // 尝试去读一个只写对象
        public const uint ABORT_CODE7 = (0x06010002);       // 尝试去写一个只读对象
        public const uint ABORT_CODE8 = (0x06020000);       // 对象字典中不存在该对象
        public const uint ABORT_CODE9 = (0x06040041);       // 该对象不能映射成PDO
        public const uint ABORT_CODE10 = (0x06040042);      // 对象映射成PDO超出 PDO长度
        public const uint ABORT_CODE11 = (0x06040043);      // 通用参数非法
        public const uint ABORT_CODE12 = (0x06040047);      // 设备内不兼容
        public const uint ABORT_CODE13 = (0x06060000);      // 由于硬件原因访问失败
        public const uint ABORT_CODE14 = (0x06070010);      // 数据类型不匹配，长度参数
        public const uint ABORT_CODE15 = (0x06070012);      // 数据类型不匹配，长度太大
        public const uint ABORT_CODE16 = (0x06070013);      // 数据类型不匹配，长度太小
        public const uint ABORT_CODE17 = (0x06090011);      // 该对象子索引不存在
        public const uint ABORT_CODE18 = (0x06090030);      // 参数超出范围
        public const uint ABORT_CODE19 = (0x06090031);      // 参数超出范围太大
        public const uint ABORT_CODE20 = (0x06090032);      // 参数超出范围太小
        public const uint ABORT_CODE21 = (0x06090036);      // 最大值小于最小值
        public const uint ABORT_CODE22 = (0x08000000);      // 一般错误
        public const uint ABORT_CODE23 = (0x08000020);      // 数据不能被传输或被保存
        public const uint ABORT_CODE24 = (0x08000021);      // 数据不能被传输或被保存由于本地控制
        public const uint ABORT_CODE25 = (0x08000022);      // 数据不能被传输或被保存由于当前状态
        public const uint ABORT_CODE26 = (0x08000023);      // 缺乏对象字典或者对象字典创建失败

       
        /***********************伺服操作模式定义*******************************/
        public const uint TQ_OP_MODE = (4);     // TQ模式
        public const uint HM_OP_MODE = (6);     // 回零模式
        public const uint CSP_OP_MODE = (8);    // CSP模式
        public const uint CSV_OP_MODE = (9);    // CSV模式
        public const uint CST_OP_MODE = (10);   // CST模式

        /***********************端子板轴回零方法定义**************************/
        public const uint HOME_NLIMT_ZINDEX = (1);                  // 负限位+Z信号
        public const uint HOME_PLIMT_ZINDEX = (2);	                // 正限位+Z信号
        public const uint HOME_PHOME_FEDGE_ZINDEX = (3);	        // 正原点开关下降沿+Z信号
        public const uint HOME_PHOME_REDGE_ZINDEX = (4);	        // 正原点开关上升沿+Z信号
        public const uint HOME_NHOME_FEDGE_ZINDEX = (5);            // 负原点开关下降沿+Z信号
        public const uint HOME_NHOME_REDGE_ZINDEX = (6);	        // 负原点开关上升沿+Z信号
        public const uint HOME_PLIMT_PHOME_FEDGE_ZINDEX = (7);	    // 正限位+正原点开关下降沿+Z信号
        public const uint HOME_PLIMT_PHOME_REDGE_ZINDEX = (8);	    // 正限位+正原点开关上升沿+Z信号
        public const uint HOME_PLIMT_NHOME_REDGE_ZINDEX = (9);		// 正限位+负原点开关上升沿+Z信号
        public const uint HOME_PLIMT_NHOME_FEDGE_ZINDEX = (10);     // 正限位+负原点开关下降沿+Z信号
        public const uint HOME_NLIMT_NHOME_FEDGE_ZINDEX = (11);	    // 负限位+负原点开关下降沿+Z信号
        public const uint HOME_NLIMT_NHOME_REDGE_ZINDEX = (12);	    // 负限位+负原点开关上升沿+Z信号
        public const uint HOME_NLIMT_PHOME_REDGE_ZINDEX = (13);	    // 负限位+正原点开关上升沿+Z信号
        public const uint HOME_NLIMT_PHOME_FEDGE_ZINDEX = (14);	    // 负限位+正原点开关下降沿+Z信号
        public const uint HOME_NLIMT = (17);	                    // 负限位
        public const uint HOME_PLIMT = (18);	                    // 正限位
        public const uint HOME_PHOME_FEDGE = (19);	                // 正原点开关下降沿
        public const uint HOME_PHOME_REDGE = (20);	                // 正原点开关上升沿
        public const uint HOME_NHOME_FEDGE = (21);	                // 负原点开关下降沿
        public const uint HOME_NHOME_REDGE = (22);	                // 负原点开关上升沿
        public const uint HOME_PLIMT_PHOME_FEDGE = (23);	        // 正限位+正原点开关下降沿
        public const uint HOME_PLIMT_PHOME_REDGE = (24);	        // 正限位+正原点开关上升沿
        public const uint HOME_PLIMT_NHOME_REDGE = (25);	        // 正限位+负原点开关上升沿
        public const uint HOME_PLIMT_NHOME_FEDGE = (26);	        // 正限位+负原点开关下降沿
        public const uint HOME_NLIMT_NHOME_FEDGE = (27);            // 负限位+负原点开关下降沿
        public const uint HOME_NLIMT_NHOME_REDGE = (28);	        // 负限位+负原点开关上升沿
        public const uint HOME_NLIMT_PHOME_REDGE = (29);	        // 负限位+正原点开关上升沿
        public const uint HOME_NLIMT_PHOME_FEDGE = (30);	        // 负限位+正原点开关下降沿
        public const uint HOME_NEGZINDEX = (33);	                // 负向Z信号
        public const uint HOME_POSZINDEX = (34);	                // 正向Z信号

        /******************回原运行状态定义*****************************/
        public const Int16 HOME_IN_PROGRESS = (0);                  // 正在回零中
        public const Int16 HOME_INTERRUPTED_OR_NOT_START = (1);     // 回零中断或者没有开始启动
        public const Int16 HOME_ATTAINED_BUT_NOT_REACH = (2);       // 回零结束，但没有到设定的目标位置
        public const Int16 HOME_SUCESS = (3);                       // 回零成功
        public const Int16 HOME_ERR_VEL_NOT_ZERO = (4);             // 回零中发生错误，同时速度不为0 
        public const Int16 HOME_ERR_VEL_ZERO = (5);                 // 回零中发生错误，同时速度为0

        /*********************资源类型宏定义*******************************
       ******* 说明：以下宏定义针对IMC_GetResCount函数********************/
        public const uint MC_ECAT_DO = (0);     // ecat的通用do
        public const uint MC_LOCAL_DO = (1);    // localBus的通用do
        public const uint MC_ECAT_DI = (11);    // ecat的通用DI
        public const uint MC_ECAT_AD = (12);    // ecat的通用AD
        public const uint MC_ECAT_DA = (13);    // ecat的通用DA
        public const uint MC_ECAT_PT = (14);    // ecat的通用PT
        public const uint MC_ECAT_AXIS = (15);  // ecat的通用AXIS
        public const uint RES_LOCAL_DO = (16);	// 本地DO数量
        public const uint MC_AXIS = (30);	    // 板卡最大轴数
        public const uint MC_PROFILE = (31);    // 板卡规划轴数
        public const uint MC_CRD_MAX_CNT = (60);// 坐标系最大个数
        public const uint MC_CRD_BUF_LEN = (61);// 坐标系缓冲区长度

        /*********************轴DI停止类型定义******************************************
        *** 说明：以下宏定义针对IMC_AxSetStopTrigPara、IMC_AxGetStopTrigPara函数********/
        public const uint CNST_DI_STOP_TYPE_ECATDI = (0);       // EcatDI停止类型
        public const uint CNST_DI_STOP_TYPE_PROBLE1_RF = (1);   // 探针1上升沿或下降沿停止
        public const uint CNST_DI_STOP_TYPE_PROBLE1_R = (2);	// 探针1上升沿停止
        public const uint CNST_DI_STOP_TYPE_PROBLE1_F = (3);    // 探针1下降沿停止


        /*********************轴状态位定义*******************************************
        ********** 说明：以下宏定义针对IMC_GetAxSts函数******************************/
        public const uint AX_ALARM_BIT = (0x00000001);          // 轴驱动报警
        public const uint AX_SVON_BIT = (0x00000002);		    // 伺服使能
        public const uint AX_BUSY_BIT = (0x00000004);		    // 轴忙状态
        public const uint AX_ARRIVE_BIT = (0x00000008);	        // 轴到位状态
        public const uint AX_POSLMT_BIT = (0x00000010);		    // 正硬限位报警
        public const uint AX_NEGLMT_BIT = (0x00000020);	        // 负硬限位报警
        public const uint AX_SOFT_POSLMT_BIT = (0x00000040);    // 正软限位报警
        public const uint AX_SOFT_NEGLMT_BIT = (0x00000080);    // 负软限位报警
        public const uint AX_ERRPOS_BIT = (0x00000100);	        // 轴位置误差越限标志
        public const uint AX_EMG_STOP_BIT = (0x00000200);		// 运动急停标志
        public const uint AX_ECAT_BIT = (0x00000400);		    // 总线轴标志

        /******************采样数据触发类型（trigType）定义**************/
        public const uint SAMPLE_TRIG_IMMEDIATE = (0);  // 立即采集
        public const uint SAMPLE_TRIG_DELAY = (1);      // 延时采集
        public const uint SAMPLE_TRIG_LOCAL_DI = (2);   // 本地DI触发
        public const uint SAMPLE_TRIG_ECAT_DI = (3);    // ECAT 的DI 触发

        /******************采样数据类型定义*****************************/
        // 单轴数据类型
        public const uint SAMPLE_ADDRESS_TYPE_AX_PRF_POS = (0x01);
        public const uint SAMPLE_ADDRESS_TYPE_AX_ENC_POS = (0x02);
        public const uint SAMPLE_ADDRESS_TYPE_AX_PRF_VEL = (0x03);
        public const uint SAMPLE_ADDRESS_TYPE_AX_ENC_VEL = (0x04);
        public const uint SAMPLE_ADDRESS_TYPE_AX_PRF_ACC = (0x05);
        public const uint SAMPLE_ADDRESS_TYPE_AX_ENC_ACC = (0x06);

        public const uint SAMPLE_ADDRESS_TYPE_PRF_POS = (0x07);
        public const uint SAMPLE_ADDRESS_TYPE_PRF_POS1 = (0x08);
        public const uint SAMPLE_ADDRESS_TYPE_PRF_POS2 = (0x09);
        public const uint SAMPLE_ADDRESS_TYPE_AX_TORQ = (0x0a);

        // 插补数据类型
        public const uint SAMPLE_ADDRESS_TYPE_CRD1_POSX = (0x100);
        public const uint SAMPLE_ADDRESS_TYPE_CRD1_POSY = (0x101);
        public const uint SAMPLE_ADDRESS_TYPE_CRD1_POSZ = (0x102);
        public const uint SAMPLE_ADDRESS_TYPE_CRD1_VEL = (0x103);

        public const uint SAMPLE_ADDRESS_TYPE_CRD2_POSX = (0x150);
        public const uint SAMPLE_ADDRESS_TYPE_CRD2_POSY = (0x151);
        public const uint SAMPLE_ADDRESS_TYPE_CRD2_POSZ = (0x152);
        public const uint SAMPLE_ADDRESS_TYPE_CRD2_VEL = (0x153);

        public const uint SAMPLE_ADDRESS_TYPE_CRD3_POSX = (0x200);
        public const uint SAMPLE_ADDRESS_TYPE_CRD3_POSY = (0x201);
        public const uint SAMPLE_ADDRESS_TYPE_CRD3_POSZ = (0x202);
        public const uint SAMPLE_ADDRESS_TYPE_CRD3_VEL = (0x203);

        public const uint SAMPLE_ADDRESS_TYPE_CRD4_POSX = (0x250);
        public const uint SAMPLE_ADDRESS_TYPE_CRD4_POSY = (0x251);
        public const uint SAMPLE_ADDRESS_TYPE_CRD4_POSZ = (0x252);
        public const uint SAMPLE_ADDRESS_TYPE_CRD4_VEL = (0x253);


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    端子板探针捕获类型 定义          
      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        public const uint CAPT_MODE_INDEX   = (0);	    // Index捕获
        public const uint CAPT_MODE_HOME    = (1);      // Home捕获
        public const uint CAPT_MODE_PROBE   = (3);	    // 探针捕获
        public const uint CAPT_MODE_LIMITP  = (4);	    // 正限位捕获
        public const uint CAPT_MODE_LIMITN  = (5);      // 负限位捕获


        /************************端子板专用IO的定义***************************/
        public const uint SPECIAL_IO_RDY = (0);         // 准备完成信号
        public const uint SPECIAL_IO_ARRIV = (1);       // 到位信号
        public const uint SPECIAL_IO_ALARM = (2);       // 报警信号
        public const uint SPECIAL_IO_POSLMT = (3);      // 正限位信号
        public const uint SPECIAL_IO_NEGLMT = (4);      // 负限位信号
        public const uint SPECIAL_IO_CLR = (5);         // 清除报警信号
        public const uint SPECIAL_IO_SV = (6);          // 伺服使能信号
        public const uint SPECIAL_IO_HOME = (7);        // 回零输入信号
        public const uint SPECIAL_IO_INDEX = (8);       // 电机Z相信号

        ///*==========================================================================*/
        ///*----             FUNCTION DEFINE                                       ---*/
        ///*==========================================================================*/

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			 板卡的操作以及资源获取接口	 			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //获取当前板卡数量
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCardsNum", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCardsNum(ref Int32 cardsNum);
        //开启运动控制卡（包含初始化过程建立总线op状态，需后台下载好配置文件）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_OpenCard", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_OpenCard(Int32 cardNo, ref HANDLETYPE pCardHandle, Int32 blockFlag);
        //关闭运动控制卡（通过句柄，总线状态断开）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CloseCard", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CloseCard(HANDLETYPE cardHandle);
        //按卡号关闭卡
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CloseCardNo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CloseCardNo(Int16 cardNo);
		//开启运动控制卡（获取控制句柄）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_OpenCardHandle", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_OpenCardHandle(Int32 cardNo, ref HANDLETYPE pCardHandle);
        //关闭运动控制卡（保留总线状态）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CloseCardHandle", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CloseCardHandle(HANDLETYPE cardHandle);
        //扫描从站并建立总线连接
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ScanCardECAT", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ScanCardECAT(HANDLETYPE cardHandle, Int32 blockFlag);
        //扫描从站并建立总线连接
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ScanCard", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ScanCard(HANDLETYPE cardHandle, Int32 blockFlag);
        //获取主站状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetECATMasterSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetECATMasterSts(HANDLETYPE cardHandle, ref UInt32 pStatus);
        //获取主站状态(扩展指令)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetECATMasterStsEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetECATMasterStsEx(HANDLETYPE cardHandle, ref UInt32 pStatus);
        //获取控制卡状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCardSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCardSts(HANDLETYPE cardHandle, ref UInt32 pStatus);
        //获取总线资源数量
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCardResource", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCardResource(HANDLETYPE cardHandle, ref TRsouresNum pRsouresNum);
        //上传设备配置文件
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpLoadDeviceConfig", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpLoadDeviceConfig(HANDLETYPE cardHandle, string pathName);
        //下载设备配置文件
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DownLoadDeviceConfig", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DownLoadDeviceConfig(HANDLETYPE cardHandle, string pathName);
        //下载系统配置文件
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DownLoadSystemConfig", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DownLoadSystemConfig(HANDLETYPE cardHandle, string pathName);
             
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			 轴参数设置接口 						    ~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置轴的激活状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxActive", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxActive(HANDLETYPE cardHandle, Int16 axNo, Int16 active);
        //获取轴的激活状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxActive", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxActive(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pActive);
        //设置控制器单轴安全运行参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxMaxMtPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxMaxMtPara(HANDLETYPE cardHandle, Int16 axNo, ref TMtPara pMtPara);
        //获取控制器单轴安全运行参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxMaxMtPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxMaxMtPara(HANDLETYPE cardHandle, Int16 axNo, ref TMtPara pMtPara);
        //设置轴当量参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxEquiv", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxEquiv(HANDLETYPE cardHandle, Int16 axNo, double[] pAxEquArray, Int16 count = 1);
        //获取轴当量参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEquiv", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEquiv(HANDLETYPE cardHandle, Int16 axNo, double[] pAxEquArray, Int16 count = 1);
        //轴输出绑定设置，根据轴类型和输出通道进行绑定
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxBondCfg", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxBondCfg(HANDLETYPE cardHandle, Int16 axNo, Int16 axType, Int16 outputChn, Int16 loaclEncSrc);
        //获取轴绑定状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxBondCfg", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxBondCfg(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pAxType, ref Int16 pOutputChn, ref Int16 pLoaclEncSrc);
        //复位轴绑定状态，让所有轴恢复到虚轴状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ResetAxBondCfg", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ResetAxBondCfg(HANDLETYPE cardHandle);
        //设置轴属性参数，包含软限位、到位误差、最大跟随误差
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxAttriPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxAttriPara(HANDLETYPE cardHandle, Int16 axNo, ref TAxAttriPara pAxAttriPara);
        //获取轴属性参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxAttriPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxAttriPara(HANDLETYPE cardHandle, Int16 axNo, ref TAxAttriPara pAxAttriPara);
        //设置轴软限位
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxSoftLimit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxSoftLimit(HANDLETYPE cardHandle, Int16 axNo, Int32 softPosLimitPos, Int32 softNegLimitPos);
        //获取轴软限位
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxSoftLimit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxSoftLimit(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pSoftPosLimitPos, ref Int32 pSoftNegLimitPos);
        //设置轴到位误差检查参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxArrivalBand", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxArrivalBand(HANDLETYPE cardHandle, Int16 axNo, Int16 arrivalBand, Int16 arrivalTime);
        //获取轴到位误差检查参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxArrivalBand", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxArrivalBand(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pArrivalBand, ref Int16 pArrivalTime);
        //设置轴最大跟随误差
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxErrorPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxErrorPos(HANDLETYPE cardHandle, Int16 axNo, Int32 errorPos);
        //获取轴最大跟随误差
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxErrorPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxErrorPos(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pErrorPos);
        //设置轴的反向背隙补偿参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxBacklash", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxBacklash(HANDLETYPE cardHandle, Int16 axNo, Int32 wholeCmpVal, Int32 cmpVel, Int16 cmpDir);
        //获取轴的反相背隙补偿参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxBacklash", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxBacklash(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pWholeCmpVal, ref Int32 pCmpVel, ref Int16 pCmpDir);
        //设置轴的安全检查是否有效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxSafeCheckEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxSafeCheckEnable(HANDLETYPE cardHandle, Int16 axNo, ref TAxCheckEn pAxCheckEn);
        //获取轴的安全检查是否有效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxSafeCheckSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxSafeCheckSts(HANDLETYPE cardHandle, Int16 axNo, ref TAxCheckEn pAxCheckEn);
        //设置轴报警有效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxAlarmEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxAlarmEnable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置轴报警无效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxAlarmDisable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxAlarmDisable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置软限位有效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxSoftLmtsEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxSoftLmtsEnable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置软限位无效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxSoftLmtsDisable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxSoftLmtsDisable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置硬限位有效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxHwLmtsEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxHwLmtsEnable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置硬限位无效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxHwLmtsDisable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxHwLmtsDisable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置跟随误差有效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxErrPosEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxErrPosEnable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置跟随误差无效
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxErrPosDisable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxErrPosDisable(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //设置探针触发位在轴DigitalInput中的bit位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxProbeMaskBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxProbeMaskBit(HANDLETYPE cardHandle, Int16 axNo, Int16 prbDiBitNo);
        //获取探针触发位在轴DigitalInput中的bit位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxProbeMaskBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxProbeMaskBit(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pPrbDiBitNo);
        //设置轴停止减速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxStopDec", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxStopDec(HANDLETYPE cardHandle, Int16 axNo, double decSmoothStop, double decAbruptStop);
        //获取轴停止减速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxStopDec", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxStopDec(HANDLETYPE cardHandle, ref double decSmoothStop, ref double decAbruptStop);
        //获取轴最大急停时间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEmgMaxDecLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEmgMaxDecLmt(HANDLETYPE cardHandle, Int16 axNo, ref UInt16 pDecLmtTime);
        //设置轴最大急停时间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxEmgMaxDecLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxEmgMaxDecLmt(HANDLETYPE cardHandle, Int16 axNo, UInt16 decLmtTime);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			 控制卡系统安全运行参数设置操作 			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置用户密码
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_WriteUserCode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_WriteUserCode(HANDLETYPE cardHandle, string pCodeArray, Int16 len);
        //校验用户密码
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CheckUserCode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CheckUserCode(HANDLETYPE cardHandle, string pCodeArray, Int16 len);
        //写入用户数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_WriteUserData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_WriteUserData(HANDLETYPE cardHandle, Int16 offset, string pCodeArray, Int16 len);
        //读取用户数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ReadUserData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ReadUserData(HANDLETYPE cardHandle, Int16 offset, string pCodeArray, Int16 len);
        //设置急停停止模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgStopMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEmgStopMode(HANDLETYPE cardHandle, Int16 stopMode);
        //获取急停停止模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEmgStopMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEmgStopMode(HANDLETYPE cardHandle, ref Int16 pStopMode);
        //设置急停信号的滤波系数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgFilter", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEmgFilter(HANDLETYPE cardHandle, Int16 filter);
        //获取急停信号的滤波系数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEmgFilter", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEmgFilter(HANDLETYPE cardHandle, ref Int16 pFilter);
        //设置急停信号触发的电平取反
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgTrigLevelInv", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEmgTrigLevelInv(HANDLETYPE cardHandle, Int16 inverse);
        //获取急停信号触发的电平取反状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEmgTrigLevelInv", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEmgTrigLevelInv(HANDLETYPE cardHandle, ref Int16 pInverse);
        //获取急停信号输入电平
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEmgDiLevel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEmgDiLevel(HANDLETYPE cardHandle, ref Int16 pLevel);
        //设置急停信号触发复位DO信号输出
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgDoResetFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEmgDoResetFlag(HANDLETYPE cardHandle, Int16 enable);
        //获取急停信号触发复位DO信号输出状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgDoResetFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEmgDoResetFlag(HANDLETYPE cardHandle, ref Int16 pEnFlag);
        //设置急停信号触发轴延时下使能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgDoResetFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEmgSvOffVelThreshold(HANDLETYPE cardHandle, Int16 axNo, Int32 thrVel);
        //获取急停信号触发轴延时下使能状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEmgDoResetFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEmgSvOffVelThreshold(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pThrVel);
        //轴警告状态映射到轴报警
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnableCheckEcatErrCode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnableCheckEcatErrCode(HANDLETYPE cardHandle, Int16 flag);
        //打开看门狗功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_OpenWatchDog", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_OpenWatchDog(HANDLETYPE cardHandle, Int32 feedTime);
        //看门狗喂狗操作
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_FeedWatchDog", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_FeedWatchDog(HANDLETYPE cardHandle);
        //关闭看门狗功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CloseWatchDog", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CloseWatchDog(HANDLETYPE cardHandle);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			 EtherCAT总线操作以及状态查询 				
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //获取EtherCAT从站当前通讯状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSlaveCurSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSlaveCurSts(HANDLETYPE cardHandle, UInt16 Station_id, UInt16 cnt, Int16[] pStatus);
        //获取EtherCAT从站当前通讯状态(拓展指令)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSlaveCurStsEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSlaveCurStsEx(HANDLETYPE cardHandle, UInt16 Station_id, UInt16 cnt, Int16[] pStatus);
        //获取EtherCAT从站通讯请求状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSlaveReqSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSlaveReqSts(HANDLETYPE cardHandle, UInt16 Station_id, UInt16 cnt, Int16[] pStatus);
        //获取EtherCAT从站通讯错误码
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatErrCode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatErrCode(HANDLETYPE cardHandle, ref UInt32 pErrCode);
        //获取EtherCAT从站通讯错误码(拓展指令)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatErrCodeEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatErrCodeEx(HANDLETYPE cardHandle, ref UInt32 pErrCode);
        // 获取从站信息
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatSlaveInfo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatSlaveInfo(HANDLETYPE cardHandle, UInt16 slaveIndex, ref TSlaveInfo ptSlaveInfo);
        // 获取主站信息
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatMasterInfo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatMasterInfo(HANDLETYPE cardHandle, ref TMasterInfo ptMasterInfo);

        // EtherCATLog记录功能开启与关闭(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EtherCATLogEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EtherCATLogEnable(HANDLETYPE cardHandle, Int16 enable, Int16 time);
        // 获取EtherCATLog信息(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UploadEtherCATLog", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UploadEtherCATLog(HANDLETYPE cardHandle, string pathName);
        // 设置轴状态机切换等待时间（V1.12.2.0及以上固件）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxisOnThresHold", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxisOnThresHold(HANDLETYPE cardHandle, Int16 axNo, UInt16 waitTime);
        // 获取轴状态机切换等待时间（V1.12.2.0及以上固件）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxisOnThresHold", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxisOnThresHold(HANDLETYPE cardHandle, Int16 axNo, ref UInt16 waitTime);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		        EtherCAT PDO操作功能 			
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        // 获取ecat轴FreePdo数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatPdoCfgInfo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatPdoCfgInfo(HANDLETYPE cardHandle);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatFreePdoData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatFreePdoData(HANDLETYPE cardHandle, Int16 station_id, UInt16 index, UInt16 subindex, Byte[] data, UInt32 data_size);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatFreePdoData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatFreePdoData(HANDLETYPE cardHandle, Int16 station_id, UInt16 index, UInt16 subindex, Byte[] data, UInt32 data_size);

        // 获取ecat轴pdo数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxisPdoData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxisPdoData(HANDLETYPE cardHandle, Int16 axNo, UInt16 index, Byte[] data);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxisPdoData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxisPdoData(HANDLETYPE cardHandle, Int16 axNo, UInt16 index, Byte[] data);
        // 获取ecat从站pdo数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatSlavePdoData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatSlavePdoData(HANDLETYPE cardHandle, Int16 station_id, UInt16 index, UInt16 subindex, Byte[] data);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatSlavePdoData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatSlavePdoData(HANDLETYPE cardHandle, Int16 station_id, UInt16 index, UInt16 subindex, Byte[] data);
        
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
		        EtherCAT SDO操作功能
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //通过轴号获取轴设备所在的站点号
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEcatStation", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEcatStation(HANDLETYPE cardHandle, UInt16 virAxNo, ref Int16 pPhyStation_id, ref Int16 pPhySlot_id);
        //通过轴号获取轴设备的设备产商号和设备产品号（应用于EtherCAT轴）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEcatPidVid", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEcatPidVid(HANDLETYPE cardHandle, UInt16 virAxNo, ref UInt32 pPid, ref UInt32 pVid);
        //通过站点号获取所有从站类型的设备产商号和设备产品号（应用于所有类型的EtherCAT从站）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatStationPidVid", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatStationPidVid(HANDLETYPE cardHandle, UInt16 Station_id, ref UInt32 pPid, ref UInt32 pVid);
        //通过站点号获取EtherCAT从站所指定的SDO数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatSdo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatSdo(HANDLETYPE cardHandle, Int16 station_id, UInt16 index, UInt16 subindex, Byte[] target, UInt32 target_size, ref UInt32 result_size, ref UInt32 abort_code);
        //通过站点号设定EtherCAT从站所指定的SDO数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatSdo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatSdo(HANDLETYPE cardHandle, Int16 station_id, UInt16 index, UInt16 subindex, Byte[] data, UInt32 data_size, ref UInt32 abort_code);
        //通过轴号获取EtherCAT轴所指定的SDO数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxSdo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxSdo(HANDLETYPE cardHandle, Int16 virAxNo, UInt16 index, UInt16 subindex, Byte[] target, UInt32 target_size, ref UInt32 result_size, ref UInt32 abort_code);
        //通过轴号设定EtherCAT轴所指定的SDO数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxSdo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxSdo(HANDLETYPE cardHandle, Int16 virAxNo, UInt16 index, UInt16 subindex, Byte[] data, UInt32 data_size, ref UInt32 abort_code);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                ECAT总线 DI/DO  AI/AO  PT功能  			
        ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置EtherCAT的DI资源取反，按8bit为一组进行设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatGrpDiInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatGrpDiInverse(HANDLETYPE cardHandle, Int16 groupNo, Int16 inverse);
        //获取EtherCAT的DI资源取反状态，按8bit为一组进行设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatGrpDiInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatGrpDiInverse(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pInverse);
        //设置EtherCAT的DO资源取反，按8bit为一组进行设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatGrpDoInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatGrpDoInverse(HANDLETYPE cardHandle, Int16 groupNo, Int16 inverse);
        //获取EtherCAT的DO资源取反状态，按8bit为一组进行设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatGrpDoInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatGrpDoInverse(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pInverse);

        //按位设置DO输出值，设置第doNo号的 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatDoBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatDoBit(HANDLETYPE cardHandle, Int16 doNo, Int16 Value);
        //按位获取DO输出值，获取第doNo号的 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatDoBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatDoBit(HANDLETYPE cardHandle, Int16 doNo, ref Int16 pValue);
        //按组设置DO输出值，设置第groupNo组的 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatGrpDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatGrpDo(HANDLETYPE cardHandle, Int16 groupNo, Int16 Value);
        //按组获取DO输出值，获取第groupNo组的 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatGrpDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatGrpDo(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pValue);
        //按包获取DO输出值，获取从第grpNo组开始的grpCnt组的 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatPackDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatPackDo(HANDLETYPE cardHandle, Int16 startGrpNo, Int16[] pValue, Int16 grpCnt);

        //按位获取DI输入值，获取第diNo号的 DI值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatDiBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatDiBit(HANDLETYPE cardHandle, Int16 diNo, ref Int16 pValue);
        //按组获取DI输入值，获取第grpNo组的 DI值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatGrpDi", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatGrpDi(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pValue);
        //按包获取DI输入值，获取从第grpNo组开始的grpCnt组的 DI值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatPackDi", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatPackDi(HANDLETYPE cardHandle, Int16 startGrpNo, Int16[] pValue, Int16 grpCnt);
        //按位设置DO输出状态延时取反
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatDoInvDelay", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatDoInvDelay(HANDLETYPE cardHandle, Int16 doNo, Int16 doVal, Int16 delay);
        // Ecat Do断线重启后状态保持功能，默认不保持（结合实际连接从站进行操作）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatDoStsHold", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatDoStsHold(HANDLETYPE cardHandle, Int16 isHold);

        //获取第adNO号通道AD数据值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAdVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAdVal(HANDLETYPE cardHandle, Int16 adNo, ref Int16 pValue);
        //设置第daNO号通道DA的输出值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatDaVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatDaVal(HANDLETYPE cardHandle, Int16 daNo, Int16 Value);
        //获取第daNO号通道DA的设定值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatDaVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatDaVal(HANDLETYPE cardHandle, Int16 daNo, ref Int16 pValue);
        //获取第ptNo通道的PT数据值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatPtVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatPtVal(HANDLETYPE cardHandle, Int16 ptNo, ref double pValue);

        // 按组读取ecatDO和DI，可以读取连续多个组，且按字节连续排序
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatGrpDiEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatGrpDiEx(HANDLETYPE cardHandle, Int16 grpIndex, Byte[] pValue, Int16 grpCnt);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatGrpDoEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatGrpDoEx(HANDLETYPE cardHandle, Int16 grpIndex, Byte[] pValue, Int16 grpCnt);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatGrpDoEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatGrpDoEx(HANDLETYPE cardHandle, Int16 grpIndex, Byte[] pValue, Int16 grpCnt);


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			 端子板资源设置 			 			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /// 获取端子板版本信息
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetBoardVersion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetBoardVersion(HANDLETYPE cardHandle, ref Int16 pVersion, ref Int16 pMonth, ref Int16 pYear);
        //获取端子板连接状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetBoardWorkSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetBoardWorkSts(HANDLETYPE cardHandle, ref Int16 pValue);
        //设置端子板DI滤波时间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalDiFilterTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalDiFilterTime(HANDLETYPE cardHandle, Int16 diIndex, UInt16[] pFilterTime, Int16 count = 1);
        //获取端子板DI滤波时间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDiFilterTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDiFilterTime(HANDLETYPE cardHandle, Int16 diIndex, UInt16[] pFilterTime, Int16 count = 1);
        //设置端子板Home、正负硬限位以及探针信号的滤波时间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalHmLmtPrbFilterTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalHmLmtPrbFilterTime(HANDLETYPE cardHandle, Int16 homeFltTime, Int16 limitFltTime, Int16 probeFltTime);
        //获取端子板Home、正负硬限位以及探针信号的滤波时间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalHmLmtPrbFilterTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalHmLmtPrbFilterTime(HANDLETYPE cardHandle, ref Int16 pHomeFltTime, ref Int16 pLimitFltTime, ref Int16 pProbeFltTime);
        //设置端子板专用IO电平取反
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalSpecialIOInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalSpecialIOInverse(HANDLETYPE cardHandle, Int16 type, Int16 inverse);
        //获取端子板专用IO电平取反状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalSpecialIOInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalSpecialIOInverse(HANDLETYPE cardHandle, Int16 type, ref Int16 pInverse);
        //设置端子板DI取反
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalDiInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalDiInverse(HANDLETYPE cardHandle, Int16 inverse);
        //获取端子板DI取反状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDiInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDiInverse(HANDLETYPE cardHandle, ref Int16 pInverse);
        //设置端子板DO取反
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalDoInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalDoInverse(HANDLETYPE cardHandle, Int16 inverse);
        //获取端子板DO取反状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDoInverse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDoInverse(HANDLETYPE cardHandle, ref Int16 pInverse);
        //打开端子板DO脉冲输出控制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnableDoBitPulse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnableDoBitPulse(HANDLETYPE cardHandle, Int16 doType, Int16 doIndex, UInt16 highLevelTime, UInt16 lowLevelTime, Int32 pulseNum, Int16 firstLevel);
        //关闭端子板DO脉冲输出控制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DisableDoBitPulse", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DisableDoBitPulse(HANDLETYPE cardHandle, Int16 doType, Int16 doIndex);
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			端子板脉冲轴控制操作 		  				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置端子板编码器计数方向
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalEncDir", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalEncDir(HANDLETYPE cardHandle, Int16 index, Int16[] pDirArray, Int16 count = 1);
        //获取端子板编码器计数方向
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalEncDir", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalEncDir(HANDLETYPE cardHandle, Int16 index, Int16[] pDirArray, Int16 count = 1);
        //设置端子板脉冲输出模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalPulseMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalPulseMode(HANDLETYPE cardHandle, Int16 plsIndex, Int16 mode, Int16 dirInverse);
        //获取端子板脉冲输出模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalPulseMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalPulseMode(HANDLETYPE cardHandle, Int16 plsIndex, ref Int16 pMode, ref Int16 pDirInverse);
        //获取端子板本地专用DI值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalSpecialDi", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalSpecialDi(HANDLETYPE cardHandle, Int16 type, ref Int16 pValue);
        //按位获取端子板本地专用DI值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalSpecialDiBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalSpecialDiBit(HANDLETYPE cardHandle, Int16 index, Int16 type, ref Int16 pValue);
        //设置端子板编码器滤波参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEncFilterPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEncFilterPara(HANDLETYPE cardHandle, Int16 index, Int16 filterDepth, Int16 filterCoef);
        //获取端子板编码器滤波参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEncFilterPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEncFilterPara(HANDLETYPE cardHandle, Int16 index, ref Int16 pFilterDepth, ref Int16 pFilterCoef);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			端子板本地 DIO 操作		  				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置本地端子板第doNo个 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalDoBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalDoBit(HANDLETYPE cardHandle, Int16 doNo, Int16 value);
        //获取本地端子板第doNo个 DO设定值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDoBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDoBit(HANDLETYPE cardHandle, Int16 doNo, ref Int16 pValue);
        //设置本地端子板DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalDo(HANDLETYPE cardHandle, Int16 value);
        //获取本地端子板DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDo(HANDLETYPE cardHandle, ref Int16 pValue);
        //按位获取本地端子板第diNo个 DO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDiBit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDiBit(HANDLETYPE cardHandle, Int16 diNo, ref Int16 pValue);
        //获取本地端子板DI值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalDi", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalDi(HANDLETYPE cardHandle, ref Int16 pValue);
        ////获取端子板AD值
        //[DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalADVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        //public static extern UInt32 IMC_GetLocalADVal(HANDLETYPE cardHandle, ref Int16 pValue);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			端子板编码器操作	  				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //获取本地轴内部计数器值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalCntPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalCntPos(HANDLETYPE cardHandle, Int16 index, Int32[] pCntPos, Int16 count = 1);
        //获取本地轴内部规划速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalCntVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalCntVel(HANDLETYPE cardHandle, Int16 index, Int32[] pCntVel, Int16 count = 1);
        //获取本地轴编码器计数器
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalEncPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalEncPos(HANDLETYPE cardHandle, Int16 index, Int32[] pEncPos, Int16 count = 1);
        //获取本地轴编码器速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalEncVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalEncVel(HANDLETYPE cardHandle, Int16 index, Int32[] pEncVel, Int16 count = 1);
        //设置规划器值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalCntPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalCntPos(HANDLETYPE cardHandle, Int16 index, Int32 cntPos);
        //设置编码器值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalEncPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalEncPos(HANDLETYPE cardHandle, Int16 index, Int32 encPos);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				端子板PWM输出控制    		  			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置PWM的输出参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPwmPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPwmPara(HANDLETYPE cardHandle, Int16 chn, Int16 pwmMode, Int32 frequency, double dutyRatio);
        //获取PWM的输出参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPwmPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPwmPara(HANDLETYPE cardHandle, Int16 chn, ref Int16 pPwmMode, ref Int32 pFrequency, ref double pDutyRatio);
        //设置PWM的输出频率（频率模式下设置有效）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPwmFrq", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPwmFrq(HANDLETYPE cardHandle, Int16 chn, Int32 frequency);
        //设置PWM的占空比（占空比模式下设置有效）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPwmDuty", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPwmDuty(HANDLETYPE cardHandle, Int16 chn, double dutyRatio);
        //设置PWM的开关
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPwmOnOff", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPwmOnOff(HANDLETYPE cardHandle, Int16 chn, Int16 onOff);
        //设置PWM的开延时
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPwmOnDelay", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPwmOnDelay(HANDLETYPE cardHandle, Int16 chn, UInt16 onDelay);
        //设置PWM的关延时
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPwmOffDelay", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPwmOffDelay(HANDLETYPE cardHandle, Int16 chn, UInt16 offDelay);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			端子板位置比较输出操作    		  		~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //位置比较输出源的配置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CompareSrcConfig", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CompareSrcConfig(HANDLETYPE cardHandle, Int16 portNo, Int16 dimension, Int16[] pCompSrc, Int16[] pCmpType);
        //位置比较物理信号输出的配置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CompareOutputConfig", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CompareOutputConfig(HANDLETYPE cardHandle, Int16 portNo, Int16 ctrlMode, Int16 stLevel, Int16 outputType, Int16 pulseWidth);
        //查询位置比较输出的状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCompareStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCompareStatus(HANDLETYPE cardHandle, Int16 portNo, ref Int16 pCmpSts, ref Int32 pCmpCount);
        //停止位置比较输出
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopCompareOut(HANDLETYPE cardHandle, Int16 portNo);
        //启动位置比较输出
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartCompareOut(HANDLETYPE cardHandle, Int16 portNo);
        //在指定端口手动输出电平或者脉冲
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CompareManualOutput", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CompareManualOutput(HANDLETYPE cardHandle, Int16 portNo, Int16 outVal);
        //一维位置比较手动模式下可控多个脉冲指令(V1.12.6.0及以上固件,V1.02及以上端子板固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CompareManualOutputNumPul", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CompareManualOutputNumPul(HANDLETYPE cardHandle, Int16 portNo, Int16 pulseNum, Int16 pulseCycle);
        //设置一维等距线性位置比较输出
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_LinearCompare", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_LinearCompare(HANDLETYPE cardHandle, Int16 portNo, Int32 intervalLen, Int32 compTimes);
        //按设置一维位置比较输出的位置数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetCompareData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetCompareData(HANDLETYPE cardHandle, Int16 portNo, Int16 compCount, Int32[] pPosBuf);
        //设置比较的数据类型
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetCompareDataType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetCompareDataType(HANDLETYPE cardHandle, Int16 portNo, Int16 type);
        //获取比较的数据类型
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCompareDataType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCompareDataType(HANDLETYPE cardHandle, Int16 portNo, ref Int16 pType);
        //设置比较位置的类型（仅适用于 1 维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetComparePosType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetComparePosType(HANDLETYPE cardHandle, Int16 portNo, Int16 type);
        //获取比较位置的类型（仅适用于 1 维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetComparePosType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetComparePosType(HANDLETYPE cardHandle, Int16 portNo, ref Int16 pType);
        //设置位置比较输出端口映射到EX-O bit0~3（仅限一维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalGpoUseType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalGpoUseType(HANDLETYPE cardHandle, Int16 index, Int16 type);
        //获取本地DO bit0~3的使用类型（仅限一维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalGpoUseType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalGpoUseType(HANDLETYPE cardHandle, Int16 index, ref Int16 pType);
        //设置多维比较参数（仅限多维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetMultiDimensComparePara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetMultiDimensComparePara(HANDLETYPE cardHandle, Int16 portNo, Int16 error, Int16 outpinType);
        //获取多维比较参数（仅限多维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetMultiDimensComparePara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetMultiDimensComparePara(HANDLETYPE cardHandle, Int16 portNo, ref Int16 pError, ref Int16 pOutPinTyp);
        //设置多维比较位置数据点（仅限多维位置比较）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetMultiDimensCompareData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetMultiDimensCompareData(HANDLETYPE cardHandle, Int16 portNo, TMultiCmpData[] pComparaData, Int16 count);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				端子板PSO输出控制    		  			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置PSO模式0的参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPSOMode0Para", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPSOMode0Para(HANDLETYPE cardHandle, Int16 portNo, Int16 dimension, Int16 posType, Int16 pinType, Int16[] pPsoPosIndexArray, double outPlsWidth, Int32 syncDeltaPos);
        //获取PSO模式0的参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPSOMode0Para", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPSOMode0Para(HANDLETYPE cardHandle, Int16 portNo, ref Int16 pDimension, ref Int16 pPosType, ref Int16 pPinType, ref Int16 pPsoPosIndexArray, ref double pOutPlsWidth, ref Int32 pSyncDeltaPos);
        //启动PSO功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartPSO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartPSO(HANDLETYPE cardHandle, Int16 portNo);
        //停止PSO功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopPSO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopPSO(HANDLETYPE cardHandle, Int16 portNo);




        ///*==========================================================================*/
        ///*----回原点功能接口                                             ---*/
        ///*==========================================================================*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				通用回原点操作       		  			~~~*/
        /*~~~说明：本地轴回原点参考CiA402回零方法，动作有一定差异 ~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动回零（仅当量在 HOME 运动过程中有效时使用）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartHomingInUnit", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartHomingInUnit(HANDLETYPE cardHandle, Int16 axNo, ref THomingParaInUint pHomingPara);
        //设定回原参数并启动回原点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartHoming(HANDLETYPE cardHandle, Int16 axNo, ref THomingPara pHomingPara);
        //停止回原点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 stopType);
        //获取回原点状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetHomingStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetHomingStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pStatus);
        //退出回原点模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_FinishHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_FinishHoming(HANDLETYPE cardHandle, Int16 axNo);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				CSP回零操作       		  			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动EtherCAT轴在CSP模式下的回零
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartEcatAxCSPHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartEcatAxCSPHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 method, Int16 dir, Int16 level, double hVel, double lVel, double acc, double offset);
        //停止EtherCAT轴在CSP模式下的回零
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopEcatAxCSPHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopEcatAxCSPHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 stopMode);
        //获取EtherCAT轴在CSP模式下的回零状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxCSPHomingSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxCSPHomingSts(HANDLETYPE cardHandle, Int16 axNo, ref  Int16 pHomingMethod, ref Int16 pSts);
        //退出EtherCAT轴在CSP模式下的回零模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_FinishEcatAxCSPHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_FinishEcatAxCSPHoming(HANDLETYPE cardHandle, Int16 axNo);
        //关闭回零完成后自动切换为位置模式功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CloseAutoHomingFinish", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CloseAutoHomingFinish(HANDLETYPE cardHandle);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				转矩回零操作       		  			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动转矩回原点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartAxTorqHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartAxTorqHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 tgtTorq, Int16 torqSlopeTime, UInt32 maxVel);
        //停止转矩回原点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopAxTorqHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopAxTorqHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 stopTime);
        //获取转矩回原点状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxTorqHomingSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxTorqHomingSts(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pStatus);
        //退出转矩回原点模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_FinishAxTorqHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_FinishAxTorqHoming(HANDLETYPE cardHandle, Int16 axNo);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				龙门回零操作      		  			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动龙门回原点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartGantryHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartGantryHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 method, Int16 dir, Int16 level, double hVel, double lVel, double acc, double offset);      
        //停止龙门回原点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopGantryHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopGantryHoming(HANDLETYPE cardHandle, Int16 axNo, Int16 stopMode);
        //获取龙门回原点状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetGantryHomingSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetGantryHomingSts(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pHomingMethod, ref Int16 pSts);
        //退出龙门回原点模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_FinishGantryHoming", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_FinishGantryHoming(HANDLETYPE cardHandle, Int16 axNo);



        ///*==========================================================================*/
        ///*----单轴运动功能接口                                             ---*/
        ///*==========================================================================*/

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				轴运动控制操作   				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //轴上使能控制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxServoOn", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxServoOn(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //轴下使能控制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxServoOff", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxServoOff(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //按轴号单轴停止
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxMoveStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxMoveStop(HANDLETYPE cardHandle, Int16 axNo, Int16 stopType);
        //按mask位进行对应轴停止
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxStopInBits", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxStopInBits(HANDLETYPE cardHandle, Int32 axMask, Int32 stopTypeBits);
        //设定单轴规划运动的速度、加速度和减速度。注：设置的值不应大于系统设定值最大值，否则报参数错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetSingleAxMvPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetSingleAxMvPara(HANDLETYPE cardHandle, Int16 axNo, double vel, double acc, double dec);
        //获取单轴规划运动的速度、加速度和减速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSingleAxMvPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSingleAxMvPara(HANDLETYPE cardHandle, Int16 axNo, ref double pVel, ref double pAcc, ref double pDec);
        //设定单轴速度规划类型。ratio越大，则S越接近T型速度规划，冲击也越大；反之，ratio越小，则规划越平顺，冲击越小
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetSingleAxVelType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetSingleAxVelType(HANDLETYPE cardHandle, Int16 axNo, Int16 velType, double ratio);
        //获取单轴速度规划类型
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSingleAxVelType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSingleAxVelType(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pVelType, ref double pRatio);
        //设定当前轴的位置值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxCurPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxCurPos(HANDLETYPE cardHandle, Int16 axNo, double setPos);
        //同步轴位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SyncAxPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SyncAxPos(HANDLETYPE cardHandle, Int16 axNo);
        //按通道设置轴反向取反
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxDirInv", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxDirInv(HANDLETYPE cardHandle, Int16 chn, Int16 inverse);
        //按通道获取轴反向取反状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxDirInv", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxDirInv(HANDLETYPE cardHandle, Int16 chn, ref Int16 pInverse);
        //设置轴规划补偿值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxCompenPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxCompenPos(HANDLETYPE cardHandle, Int16 axNo, double cmpPos, double cmpTime, Int16 posType);
        //获取轴规划补偿值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxCompenPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxCompenPos(HANDLETYPE cardHandle, Int16 axNo, ref double pCmpPos);
        //设置伺服轴控制模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxCtrlMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxCtrlMode(HANDLETYPE cardHandle, Int16 axNo, Int16 ctrlMode);
        //获取伺服轴控制模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxCtrlMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxCtrlMode(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pCtrlMode);
        //获取轴错误码
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxErrCode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxErrCode(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pErrorCode);
        //获取 EtherCAT 类型轴的对应的数字量输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEcatDigitalInput", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEcatDigitalInput(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pDigitalInput);
        //获取轴反向间隙的实际补偿值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxBacklashCmpVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxBacklashCmpVal(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pCmpVal);
        //设置ECAT轴本地数字量输出（仅EtherCAT轴有效）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxDo(HANDLETYPE cardHandle, Int16 axNo, Int32 axDoVal);
        //获取ECAT轴本地数字量输出状态（仅EtherCAT轴有效）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxDo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxDo(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pAxDoVal);
        // 设置DI停止操作
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxSetStopTrigPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxSetStopTrigPara(HANDLETYPE cardHandle, Int16 axNo, Int16 uselessFlag, Int16 bitNo, Int16 stopType, Int16 diType);
        // 获取DI停止操作参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxGetStopTrigPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxGetStopTrigPara(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pUselessFlag, ref Int16 pBitNo, ref Int16 pStopType, ref Int16 pDiType);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				轴状态监控操作   				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //获取轴规划模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPrfMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPrfMode(HANDLETYPE cardHandle, Int16 axNo, Int16[] pPrfMode, Int16 count = 1);
        //获取轴状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxSts(HANDLETYPE cardHandle, Int16 axNo, Int32[] pAxSts, Int16 count = 1);

        //清除轴状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ClrAxSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ClrAxSts(HANDLETYPE cardHandle, Int16 axNo, Int16 count = 1);
        //获取轴规划位置（单条最多16个轴）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPrfPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPrfPos(HANDLETYPE cardHandle, Int16 axNo, double[] pPrfPos, Int16 count = 1);
        //获取轴规划速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPrfVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPrfVel(HANDLETYPE cardHandle, Int16 axNo, double[] pPrfVel, Int16 count = 1);
        //获取轴规划加速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPrfAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPrfAcc(HANDLETYPE cardHandle, Int16 axNo, double[] pPrfAcc, Int16 count = 1);
        //获取轴反馈位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEncPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEncPos(HANDLETYPE cardHandle, Int16 axNo, double[] pEncPos, Int16 count = 1);
        //获取轴原始反馈位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxOrgEncPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxOrgEncPos(HANDLETYPE cardHandle, Int16 axNo, Int32[] pOrgEncPos, Int16 count = 1);
        //获取轴反馈速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEncVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEncVel(HANDLETYPE cardHandle, Int16 axNo, double[] pEncVel, Int16 count = 1);
        //获取轴反馈加速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEncAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEncAcc(HANDLETYPE cardHandle, Int16 axNo, double[] pEncAcc, Int16 count = 1);
        //获取规划器位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPrfPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPrfPos(HANDLETYPE cardHandle, Int16 axNo, double[] pPrfPos, Int16 count = 1);
        //获取轴类型及输出端口或站点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxType(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pAxType, ref Int16 pOutChn);
        //获取多个轴同时到位状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetMultiAxArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetMultiAxArrivalSts(HANDLETYPE cardHandle, Int32 axMask, ref Int16 pSts);


        //获取轴状态（扩展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxStsEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxStsEx(HANDLETYPE cardHandle, Int16 axNo, Int32[] AxStsEx, Int16 count = 1);
        //获取轴规划位置（扩展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPrfPosEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPrfPosEx(HANDLETYPE cardHandle, Int16 axNo, double[] pPrfPos, Int16 count = 1);
        //获取轴反馈位置（扩展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxEncPosEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxEncPosEx(HANDLETYPE cardHandle, Int16 axNo, double[] pEncPos, Int16 count = 1);


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				PTP点位运动模式接口      				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动PTP点位运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartPtpMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartPtpMove(HANDLETYPE cardHandle, Int16 axNo, double tgtPos, Int16 posType = 0);
        //启动多轴PTP点位运动。最大16轴同时启动，若有任何一轴正在运动或者报警，则调用返回忙错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartMultiPtpMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartMultiPtpMove(HANDLETYPE cardHandle, Int16 axNum, Int16[] pAxNo, double[] pTgtPos, Int16[] pPosType);
        //更新PTP点位运动目标位置，可在运行中调用
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpdatePtpTgtPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpdatePtpTgtPos(HANDLETYPE cardHandle, Int16 axNo, double tgtPos);
        //更新PTP点位运动目标速度与加减速度。可在运行中调用  注：设置的值不应大于系统设定值最大值，否则报参数错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpdatePtpMvPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpdatePtpMvPara(HANDLETYPE cardHandle, Int16 axNo, double tgtVel, double acc, double dec);
        //更新PTP点位运动目标速度。可在运行中调用  注：设置的值不应大于系统设定值最大值，否则报参数错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpdatePtpTgtVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpdatePtpTgtVel(HANDLETYPE cardHandle, Int16 axNo, double tgtVel);
        //暂停PTP运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PauseMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PauseMove(HANDLETYPE cardHandle, Int16 axNo);
        //恢复PTP运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ResumeMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ResumeMove(HANDLETYPE cardHandle, Int16 axNo);
        //获取PTP点位运动目标位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPtpTgtPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPtpTgtPos(HANDLETYPE cardHandle, Int16 axNo, ref double pTgtPos);
        //启动PTP点位运动(拓展指令)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartPtpMove_Ex", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartPtpMove_Ex(HANDLETYPE cardHandle, Int16 axNo, double tgtPos, Int16 posType = 0);

      

        //轴停机故障原因检查指令（新增指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxStopReason", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxStopReason(HANDLETYPE cardHandle, Int16 axNo, Int16[] pAxStopReason, Int16 count = 1);
        //*pAxStopReason  的值代表的具体含义为：
        //	0.正常停止；
        //	1.	急停信号触发；
        //	2.	掉线；
        //	3.	看门狗触发；
        //	4.	正硬限位触发；
        //	5.	负硬限位触发；
        //	6.	正软限位触发；
        //	7.	负软限位触发；
        //	8.	跟随误差触发；
        //	9.	伺服报警；
        //	10.轴有异常状态置起；
        //	11.DI信号触发停止；
        //	12.探针触发；
        //	13.运动模式切换触发；
        //	14.调用IMC_AxMoveStop触发；

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				Jog运动模式接口     					~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动Jog运动。若当前轴正在运动，则调用返回忙错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartJogMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartJogMove(HANDLETYPE cardHandle, Int16 axNo, double tgVel);
        //启动多轴Jog运动。最大16轴同时启动，若当前轴正在运动或者报警，则调用返回忙错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartMultiJogMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartMultiJogMove(HANDLETYPE cardHandle, Int16 axNum, Int16[] pAxNo, double[] pTgtVel);
        //更新Jog运动的目标速度。可在运行中修改；注：设置的值不应大于系统设定值最大值，否则报参数错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpdateJogTgtVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpdateJogTgtVel(HANDLETYPE cardHandle, Int16 axNo, double tgVel);
        //更新Jog运动运行参数。可在运行中修改；注：设置的值不应大于系统设定值最大值，否则报参数错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpdateJogMvPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpdateJogMvPara(HANDLETYPE cardHandle, Int16 axNo, double tgVel, double acc, double dec);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				手轮跟随运动模式接口     				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //进入手轮跟随模式。若当前轴正在运动，则调用返回忙错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnterHandWheelMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnterHandWheelMode(HANDLETYPE cardHandle, Int16 axNo, Int16 masterType, Int16 masterIndex, double ratio, double acc, double vel);
        //退出手轮跟随模式。若当前轴正在运动，则调用返回忙错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ExitHandWheelMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ExitHandWheelMode(HANDLETYPE cardHandle);
        //切换手轮速度倍率。若当前轴非手脉模式，则调用返回忙错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SwitchHandWheelRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SwitchHandWheelRatio(HANDLETYPE cardHandle, double ratio);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				位置捕获功能接口   					~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置轴位置捕获模式，需要根据该轴挂接的硬件进行设置参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxCaptMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxCaptMode(HANDLETYPE cardHandle, Int16 axNo, Int16 trigType, Int16 captSrc, Int16 sns);
        //获取轴位置捕获模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxCaptMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxCaptMode(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pTrigType, ref Int16 pCaptSrc, ref Int16 pSns);
        //获取单沿捕获状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxCaptStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxCaptStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pSts, ref Int32 pCaptPos);
        //获取双沿捕获状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxDbEdgCaptStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxDbEdgCaptStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pSts, Int32[] pCaptPos);
        //设置轴连续位置捕获模式（仅EtherCAT轴）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxCaptRepeat", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxCaptRepeat(HANDLETYPE cardHandle, Int16 axNo, Int16 count);
        //获取连续捕获次数与状态（仅EtherCAT轴）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxCaptRepeatStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxCaptRepeatStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pCaptCount, ref Int16 pRestCount, ref Int16 pCaptErr);
        //获取连续捕获到的位置值（仅EtherCAT轴）
   
        
        // 设置探针功能字(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetTouchProbeFunction", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetTouchProbeFunction(HANDLETYPE cardHandle, Int16 axNo, Int16 probeFunction);

        // 获取探针功能字(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetTouchProbeFunction", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetTouchProbeFunction(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pProbeFunction);

        // 获取探针状态及位置(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetTouchProbeStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetTouchProbeStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pProbeStatus, ref Int32 pProbe1PosValue, ref Int32 pProbe1NegValue, ref Int32 pProbe2PosValue, ref Int32 pProbe2NegValue);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~ 端子板连续位置捕获功能指令                      	~~~*/
        /* (V1.12.2.0及以上固件,V1.02及以上端子板固件)      	~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置端子板轴连续位置捕获功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetLocalCaptRepeatMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetLocalCaptRepeatMode(HANDLETYPE cardHandle, Int16 axNo, Int16 trigType, Int16 captSrc, Int16 trigSns, Int16 captCount);
        //获取端子板轴连续位置捕获功能参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalCaptRepeatMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalCaptRepeatMode(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pTrigType, ref Int16 pCaptSrc, ref Int16 pTrigSns, ref Int16 pCaptCount);
        //获取端子板轴连续位置捕获状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalCaptRepeatStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalCaptRepeatStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pCaptSts, ref Int16 pCaptCount);
        //获取端子板轴连续位置捕获位置值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetLocalCaptRepeatPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetLocalCaptRepeatPos(HANDLETYPE cardHandle, Int16 axNo, Int16 count, Int32[] pCaptPosArray);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			PTP连续运动模式接口       				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //按照固定时间形式压入PTP连续运动数据点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PTPCAddFixTData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PTPCAddFixTData(HANDLETYPE cardHandle, Int16 axNo, double tgtPos, double acc, double dec, double T, double finalVel, Int16 posType);
        //按照设定目标速度形式压入PTP连续运动数据点
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PTPCAddFixVmData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PTPCAddFixVmData(HANDLETYPE cardHandle, Int16 axNo, double tgtPos, double acc, double dec, double tgtVel, double finalVel, Int16 posType);
        //压入中断缓冲区运动数据，原缓冲区数据被清除，按新的缓冲区数据进行运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PTPCInterruptData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PTPCInterruptData(HANDLETYPE cardHandle, Int16 axNo, double tgtPos, double acc, double dec, double tgtVel, double finalVel, Int16 posType);
        //启动PTP连续运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PTPCStart", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PTPCStart(HANDLETYPE cardHandle, Int16 axNo);
        //获取PTP连续运动可压入的缓冲区数据空间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PTPCGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PTPCGetSpace(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pSpace);
        //清除PTP连续运动的缓冲区数据空间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PTPCClrData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PTPCClrData(HANDLETYPE cardHandle, Int16 axNo);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			周期运动模式接口							~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置周期运动参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetCycleMovePara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetCycleMovePara(HANDLETYPE cardHandle, Int16 axNo, Int32 cycleDis);
        //获取周期运动参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCycleMovePara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCycleMovePara(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pCycleDis);
        //启动周期运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartCycleMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartCycleMove(HANDLETYPE cardHandle, Int16 axNo, Int32 tgtPos, Int16 posType);
        //停止周期运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopCycleMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopCycleMove(HANDLETYPE cardHandle, Int16 axNo, Int16 stopType);
        //获取周期运动规划位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCycleMovePrfPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCycleMovePrfPos(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pPrfPos);
        //获取周期运动的编码器位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCycleMoveEncPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCycleMoveEncPos(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pEncPos);
        //设置周期运动的当前位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetCycleMoveCurPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetCycleMoveCurPos(HANDLETYPE cardHandle, Int16 axNo, Int32 setPos);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				CST转矩规划模式操作   				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //规划轴的转矩输出
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PrfAxTorq", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PrfAxTorq(HANDLETYPE cardHandle, Int16 axNo, Int16 tgtTrq, Int16 time);
        //获取当前轴的实际转矩
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxActTorq", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxActTorq(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pActTrq);
        //获取多个轴的实际转矩
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxActTorq32", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxActTorq32(HANDLETYPE cardHandle, Int16 axNo, Int16[] pActTrq, Int16 count = 1);
        //获取多个轴的实际转矩
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxActTorqEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxActTorqEx(HANDLETYPE cardHandle, Int16 axNo, Int16[] pActTrq, Int16 count = 1);
        //设置转矩斜坡
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxTorqSlope", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxTorqSlope(HANDLETYPE cardHandle, Int16 axNo, Int32 trqSlope);
        //获取转矩斜坡
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxTorqSlope", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxTorqSlope(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pTrqSlope);
        //设置 EtherCAT 类型轴的对应的目标转矩
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxTgtTorq", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxTgtTorq(HANDLETYPE cardHandle, Int16 axNo, Int16 tgtTrq);
        //获取 EtherCAT 类型轴的对应的目标转矩
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxTgtTorq", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxTgtTorq(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pTgtTrq);
        //设置最大速度限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxMaxVelLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxMaxVelLmt(HANDLETYPE cardHandle, Int16 axNo, Int32 maxVel);
        //获取最大速度限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxMaxVelLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxMaxVelLmt(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pMaxVel);
        //设置正向力矩限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxPosTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxPosTorqLmt(HANDLETYPE cardHandle, Int16 axNo, Int16 posTorqLmt);
        //获取正向力矩限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxPosTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxPosTorqLmt(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pPosTorqLmt);
        //设置负向力矩限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxNegTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxNegTorqLmt(HANDLETYPE cardHandle, Int16 axNo, Int16 negTorqLmt);
        //获取负向力矩限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxNegTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxNegTorqLmt(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pNegTorqLmt);
        //设置最大力矩限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatAxMaxTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatAxMaxTorqLmt(HANDLETYPE cardHandle, Int16 axNo, Int16 maxTorqLmt);
        //获取最大力矩限制
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEcatAxMaxTorqLmt", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEcatAxMaxTorqLmt(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pMaxTorqLmt);


        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			CSV速度规划模式操作       				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //启动轴CSV规划
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartAxCsvPrf", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartAxCsvPrf(HANDLETYPE cardHandle, Int16 axNo, Int32 tgtVel, Int32 acc, Int16 prfType);
        //平滑停止轴CSV规划
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopAxCsvPrf", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopAxCsvPrf(HANDLETYPE cardHandle, Int16 axNo, Int32 dec);
        //急停轴CSV规划
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EstopAxCsvPrf", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EstopAxCsvPrf(HANDLETYPE cardHandle, Int16 axNo);
        //更新CSV目标规划速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_UpdateAxCsvPrf", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_UpdateAxCsvPrf(HANDLETYPE cardHandle, Int16 axNo, Int32 tgtVel);
        //获取CSV的规划状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxCsvPrfStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxCsvPrfStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pStatus);


        ///*==========================================================================*/
        ///*----多轴运动功能接口                                             	---*/
        ///*==========================================================================*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				缓冲区插补运动模式接口     			~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //插补坐标系创建
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetMtSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetMtSys(HANDLETYPE cardHandle, Int16 crdNo, Int16[] pMaskAxNo, Int16 lookAheadLen, double estopDec);
        //获取插补坐标系参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetMtSysParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetMtSysParam(HANDLETYPE cardHandle, Int16 crdNo, Int16[] pMaskAxNo, ref Int16 pLookAheadLen, ref double pEstopDec);
        //删除已经创建的插补坐标系
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdDeleteMtSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdDeleteMtSys(HANDLETYPE cardHandle, Int16 crdNo);
        //设置插补高级参数，注意插补运行时无法设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetAdvParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetAdvParam(HANDLETYPE cardHandle, Int16 crdNo, ref TCrdAdvParam pCrdAdvParam);
        //获取插补高级参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetAdvParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetAdvParam(HANDLETYPE cardHandle, Int16 crdNo, ref TCrdAdvParam pCrdAdvParam);
        //设置插补平滑参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetSmoothParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetSmoothParam(HANDLETYPE cardHandle, Int16 crdNo, Int32 smoothLevel, double smoothTol);
        //获取插补平滑参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetSmoothParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetSmoothParam(HANDLETYPE cardHandle, Int16 crdNo, ref Int32 pSmoothLevel, ref double pSmoothTol);

        //插补轨迹速度设定
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetTrajVel(HANDLETYPE cardHandle, Int16 crdNo, double tgtVel);
        //插补轨迹加速度设定
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetTrajAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetTrajAcc(HANDLETYPE cardHandle, Int16 crdNo, double tgtAcc);
        //插补轨迹加减速度设定
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetTrajAccAndDec", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetTrajAccAndDec(HANDLETYPE cardHandle, Int16 crdNo, double tgtAcc, double tgtdec);
        //插补强制规划末速度降为0标识。如果将强制规划末速度降为0，则下面所有线段末速度为0
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetZeroFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetZeroFlag(HANDLETYPE cardHandle, Int16 crdNo, Int16 ZeroFlag);
        //插补运动指令的位置编程模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetIncMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetIncMode(HANDLETYPE cardHandle, Int16 crdNo, Int16 mode);
        //插补用户模式下起始和终点速度的设置（只有在用户规划模式下生效）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdUserVelPlan", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdUserVelPlan(HANDLETYPE cardHandle, Int16 crdNo, double uStartVel, double uEndVel);
        //插补设置同步轴的跟随速度模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetFolVelMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetFolVelMode(HANDLETYPE cardHandle, Int16 crdNo, Int16 mode);
        //插补获取同步轴的跟随速度模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetFolVelMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetFolVelMode(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pMode);
        //插补轨迹速度获取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetTrajVel(HANDLETYPE cardHandle, Int16 crdNo, ref double pTgtVel);
        //插补轨迹加速度和减速度获取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetTrajAccAndDec", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetTrajAccAndDec(HANDLETYPE cardHandle, Int16 crdNo, ref double pTgtAcc, ref double pTgtdec);
        //插补强制规划末速度降为 0 标识读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetZeroFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetZeroFlag(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pZeroFlag);
        //插补运动指令的位置编程模式读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetIncMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetIncMode(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pMode);
        //插补运动指令在用户规划模式下设置的运动段的始末速度的获取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetUserVelPlan", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetUserVelPlan(HANDLETYPE cardHandle, Int16 crdNo, ref double pUStartVel, ref double pUEndVel);
        //插补运动指令的过渡精度设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetTrajTol", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetTrajTol(HANDLETYPE cardHandle, Int16 crdNo, double tol);
        //插补运动指令的拐弯系数设置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetTrajTurnCoef", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetTrajTurnCoef(HANDLETYPE cardHandle, Int16 crdNo, double turnCoef);

        //3轴直线插补运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdLineXYZ", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdLineXYZ(HANDLETYPE cardHandle, Int16 crdNo, double[] pEndPos, Int32 userID = 0);
        //3轴直线插补运动(拓展指令)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdLineXYZEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdLineXYZEx(HANDLETYPE cardHandle, Int16 crdNo, double[] pEndPos, Int32 userID = 0);
        //XY平面内直线插补运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdLineXY", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdLineXY(HANDLETYPE cardHandle, Int16 crdNo, double[] pEndPos, Int32 userID = 0);
        //XY平面内直线插补运动(拓展指令)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdLineXYEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdLineXYEx(HANDLETYPE cardHandle, Int16 crdNo, double[] pEndPos, Int32 userID = 0);
        //ZX平面内直线插补运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdLineZX", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdLineZX(HANDLETYPE cardHandle, Int16 crdNo, double[] pEndPos, Int32 userID = 0);
        //YZ平面内直线插补运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdLineYZ", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdLineYZ(HANDLETYPE cardHandle, Int16 crdNo, double[] pEndPos, Int32 userID = 0);

        //给定三点的3D圆弧插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcThreePoint", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcThreePoint(HANDLETYPE cardHandle, Int16 crdNo, double[] pMidPos, double[] pEndPos, Int32 userID = 0);

        //空间arc插补
        //给定圆心，末点法向量的圆弧插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_Crd3DArcCenterNormal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_Crd3DArcCenterNormal(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double[] pEndPos, double[] pNormal, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //给定圆弧半径，圆弧末点和圆弧法向量的圆弧插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_Crd3DArcRadiusNormal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_Crd3DArcRadiusNormal(HANDLETYPE cardHandle, Int16 crdNo, double radius, double[] pEndPos, double[] pNormal, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //给定圆心和角度的圆弧插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_Crd3DArcAngleNormal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_Crd3DArcAngleNormal(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double angle, double[] pNormal, double height = 0, Int32 userID = 0);

        //平面arc插补
        //圆心终点编程
        //XY平面内圆心末点编程；Z轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcCenterXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcCenterXYPlane(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double[] pEndPos, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //YZ平面内圆心末点编程；X轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcCenterYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcCenterYZPlane(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double[] pEndPos, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //ZX平面内圆心末点编程；Y轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcCenterZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcCenterZXPlane(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double[] pEndPos, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //半径终点编程
        //XY平面内半径末点编程；Z轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcRadiusXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcRadiusXYPlane(HANDLETYPE cardHandle, Int16 crdNo, double radius, double[] pEndPos, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //YZ平面内半径末点编程；X轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcRadiusYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcRadiusYZPlane(HANDLETYPE cardHandle, Int16 crdNo, double radius, double[] pEndPos, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //YZ平面内半径末点编程；X轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcRadiusZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcRadiusZXPlane(HANDLETYPE cardHandle, Int16 crdNo, double radius, double[] pEndPos, Int16 dir, double height = 0, Int32 turn = 0, Int32 userID = 0);
        //圆心角度编程
        //XY平面内半径末点编程；Z轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcAngleXYPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcAngleXYPlane(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double angle, double height = 0, Int32 userID = 0);
        //XY平面内半径末点编程；Z轴不作圆周运动（拓展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcAngleXYPlaneEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcAngleXYPlaneEx(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double angle, double height = 0, Int32 userID = 0);
        //YZ平面内圆心角度编程；X轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcAngleYZPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcAngleYZPlane(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double angle, double height = 0, Int32 userID = 0);
        //ZX平面内圆心角度点编程；Y轴不作圆周运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdArcAngleZXPlane", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdArcAngleZXPlane(HANDLETYPE cardHandle, Int16 crdNo, double[] pCenter, double angle, double height = 0, Int32 userID = 0);
        //FIFO事件
        //插补缓冲区等待延时
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdWaitTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdWaitTime(HANDLETYPE cardHandle, Int16 crdNo, Int32 waitPeriod, Int32 userID = 0);
        //插补缓冲区等待DI
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdWaitDI", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdWaitDI(HANDLETYPE cardHandle, Int16 crdNo, Int16 diNO, Int16 diType, Int16 diLevel, Int32 userID = 0);
        //插补缓冲区输出DO
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetDO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetDO(HANDLETYPE cardHandle, Int16 crdNo, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);
        //插补缓冲区输出DO（拓展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetDOEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetDOEx(HANDLETYPE cardHandle, Int16 crdNo, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);

        //插补缓冲区按距离输出DO
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetDistanceDO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetDistanceDO(HANDLETYPE cardHandle, Int16 crdNo, double waiPos, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);
        //插补缓冲区按距离输出DO（拓展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetDistanceDOEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetDistanceDOEx(HANDLETYPE cardHandle, Int16 crdNo, double waiPos, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);
        //插补缓冲区延时输出DO
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetTimeDO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetTimeDO(HANDLETYPE cardHandle, Int16 crdNo, int waitPeriod, Int16 doNO, Int16 doType, Int16 doLevel, Int32 userID = 0);
        //插补缓冲区启动插补轴以外的轴PTP运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdPTPMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdPTPMove(HANDLETYPE cardHandle, Int16 crdNo, Int16 axNo, double tgtPos, double tgtVel, double acc, Int16 mvType, Int16 waitFlag, Int32 userID = 0);
        //插补缓冲区中启动插补轴之外的轴同步相对运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSyncMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSyncMove(HANDLETYPE cardHandle, Int16 crdNo, Int16 axNo, double syncPos, Int32 userID = 0);
        //插补缓冲区中启动插补轴之外的轴同步绝对运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSyncMoveAbs", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSyncMoveAbs(HANDLETYPE cardHandle, Int16 crdNo, Int16 axNo, double syncPos, Int32 userID = 0);
        //插补缓存区多轴同步运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdMultiSyncMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdMultiSyncMove(HANDLETYPE cardHandle, Int16 crdNo, Int16 axNum, Int16[] axNo, double[] tgtPos, double vel, double acc, Int16 contiflag, Int32 userID = 0);
        //插补缓冲区等待上一段运动到位 
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdWaitInPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdWaitInPos(HANDLETYPE cardHandle, Int16 crdNo, Int32 userID = 0);
        //插补缓存区多轴同步运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdMultiSyncMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdMultiSyncMove(HANDLETYPE cardHandle, Int16 crdNo, Int16 axNum, Int16[] pAxNo, double[] pTgtPos, double tgtVel, double tgtAcc, Int16 contiFlag);
        //往DSP队列发送PC部分的队列数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdEndData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdEndData(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pIsFinished);
        //往DSP队列发送PC部分的队列数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdEndDataEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdEndDataEx(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pIsFinished, ref Int16 pSeg);

        //启动插补运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdStart", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdStart(HANDLETYPE cardHandle, Int16 crdNo);
        //停止插补运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdStop(HANDLETYPE cardHandle, Int16 crdNo, Int16 stopType);
        //获取插补坐标系状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetStatus(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pStatus);
        //插补倍率设定函数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdSetRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdSetRatio(HANDLETYPE cardHandle, Int16 crdNo, double ratio);
        //插补倍率获取函数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetRatio(HANDLETYPE cardHandle, Int16 crdNo, ref double pRatio);
        //当前插补坐标系坐标读取函数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetPos(HANDLETYPE cardHandle, Int16 crdNo, double[] pCrdPos);
        //插补轨迹速度读取函数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetVel(HANDLETYPE cardHandle, Int16 crdNo, ref double pCrdVel);
        //插补当前运动曲线用户索引获取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetUserID", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetUserID(HANDLETYPE cardHandle, Int16 crdNo, ref Int32 pUserID);
        //获取插补缓存队列当前的剩余余量
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetSpace(HANDLETYPE cardHandle, Int16 crdNo, ref Int32 pSpace);
        //获取插补缓存队列当前的剩余余量（扩展指令）
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetSpaceEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetSpaceEx(HANDLETYPE cardHandle, Int16 crdNo, ref Int32 pSpace, ref Int32 lookAheadSpace);
        //获取CPU队列当前的剩余空间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetBufSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetBufSpace(HANDLETYPE cardHandle, Int16 crdNo, ref Int32 pBufSpace);
        //获取PC前瞻队列当前的剩余空间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetLookAheadSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetLookAheadSpace(HANDLETYPE cardHandle, Int16 crdNo, ref Int32 pLookAheadSpace);
        //清除缓存区压入曲线数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdClrData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdClrData(HANDLETYPE cardHandle, Int16 crdNo);
        //清除插补错误号
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdClrError", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdClrError(HANDLETYPE cardHandle, Int16 crdNo);
        //获取插补目标位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetTargetPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetTargetPos(HANDLETYPE cardHandle, Int16 crdNo, double[] pPos);
        //获取插补暂停的位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetPausePos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetPausePos(HANDLETYPE cardHandle, Int16 crdNo, double[] pPos);
        //获取插补运动最后一段到位状态标志
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CrdGetArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CrdGetArrivalSts(HANDLETYPE cardHandle, Int16 crdNo, ref Int16 pSts);




        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			多轴插补系统功能接口						~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //多轴插补系统建立
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiSetupSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiSetupSys(HANDLETYPE cardHandle, Int16 groupNo, Int16[] pMaskAxNo, Int16 maxAxNum);

        //删除多轴插补系统
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiDeleteSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiDeleteSys(HANDLETYPE cardHandle, Int16 groupNo);

        //多轴插补线段输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiLineMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiLineMove(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, double trajVel, double trajAcc, double trajdec, short blendType, double blendRatio, int userID = 0);

        //多轴插补DO输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiSetDO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiSetDO(HANDLETYPE cardHandle, Int16 groupNo, Int16 doIndex, Int16 doType, Int16 doLevel, Int32 waitTime, Int32 userID = 0);

        //多轴插补TimeDO输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiSetTimeDO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiSetTimeDO(HANDLETYPE cardHandle, Int16 groupNo, Int16 doIndex, Int16 doType, Int16 doLevel, Int32 waitTime, Int32 userID = 0);

        //多轴插补ReserveDO输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiSetReverseDO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiSetReverseDO(HANDLETYPE cardHandle, Int16 groupNo, Int16 doIndex, Int16 doType, Int16 doLevel, Int32 waitTime, Int32 userID = 0);

        //多轴插补DI输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiWaitDI", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiWaitDI(HANDLETYPE cardHandle, Int16 groupNo, Int16 diIndex, Int16 diType, Int16 diLevel, Int32 waitTime, Int32 userID = 0);

        //多轴插补等待输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiWaitTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiWaitTime(HANDLETYPE cardHandle, Int16 groupNo, Int32 waitTime, Int32 userID = 0);


        //多轴插补开始运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiStart", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiStart(HANDLETYPE cardHandle, Int16 groupNo);

        //多轴插补停止运动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiStop(HANDLETYPE cardHandle, Int16 groupNo, Int16 stopType);

        //多轴插补获取剩余空间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetSpace(HANDLETYPE cardHandle, Int16 groupNo, ref Int32 pSpace);

        //多轴插补清空缓冲区数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiClrData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiClrData(HANDLETYPE cardHandle, Int16 groupNo);

        //多轴插补清除报警
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiClrError", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiClrError(HANDLETYPE cardHandle, Int16 groupNo);

        //多轴插补编程模式读写
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiSetPosType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiSetPosType(HANDLETYPE cardHandle, Int16 groupNo, Int16 posType);

        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetPosType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetPosType(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pPosType);

        //多轴插补用户ID读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetUserID", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetUserID(HANDLETYPE cardHandle, Int16 groupNo, ref Int32 pUserID);

        //多轴插补状态获取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetStatus(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pStatus, ref Int16 pErrcode);

        //多轴获取到位状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetArrivalSts(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pArrivalSts);

        //多轴插补速度倍率读写
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiSetRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiSetRatio(HANDLETYPE cardHandle, Int16 groupNo, double ratio);

        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetRatio(HANDLETYPE cardHandle, Int16 groupNo, ref double pRatio);

        //多轴当前各轴位置读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetTrajPos", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetTrajPos(HANDLETYPE cardHandle, Int16 groupNo, double[] pPos);

        //多轴当前合成速度读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MultiGetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MultiGetTrajVel(HANDLETYPE cardHandle, Int16 groupNo, ref double pVel);





        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~		立即插补运动模式接口						~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //合成速度、加减速度形式的多轴直线立即插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateLineMoveInSynVelAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateLineMoveInSynVelAcc(HANDLETYPE cardHandle, Int16[] pMaskAxNo, Int16 axNum, double[] pEndPos, double trajVel, double trajAcc, double trajDec, double smoothCoef, Int16 type);
        //各自轴的实际速度加速度形式的多轴直线立即插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateLineMoveInAxisVelAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateLineMoveInAxisVelAcc(HANDLETYPE cardHandle, Int16[] pMaskAxNo, Int16 axNum, double[] pEndPos, double[] pAxVel, double[] pAxAcc, double[] pAxDec, double smoothCoef, Int16 type);
        //三点式圆弧立即插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateArcThreePointMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateArcThreePointMove(HANDLETYPE cardHandle, Int16[] pMaskAxNo, Int16 axNum, double[] pMidPos, double[] pEndPos, double trajVel, double trajAcc, double trajDec, double smoothCoef, Int16 type);
        //圆心终点式圆弧立即插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateArcCenterMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateArcCenterMove(HANDLETYPE cardHandle, Int16[] pMaskAxNo, Int16 axNum, double[] pCenter, double[] pEndPos, Int16 dir, double height, int turn, double trajVel, double trajAcc, double trajDec, double smoothCoef, Int16 type);
        //半径终点式圆弧立即插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateArcRadiusMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateArcRadiusMove(HANDLETYPE cardHandle, Int16[] pMaskAxNo, Int16 axNum, double radius, double[] pEndPos, Int16 dir, double height, int turn, double trajVel, double trajAcc, double trajDec, double smoothCoef, Int16 type);
        //圆心圆心角式圆弧立即插补
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateArcAngleMove", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateArcAngleMove(HANDLETYPE cardHandle, Int16[] pMaskAxNo, Int16 axNum, double[] pCenter, double angle, double height, double trajVel, double trajAcc, double trajDec, double smoothCoef);
        //获取多轴立即插补运动的合成运行速度
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateMoveGetVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateMoveGetVel(HANDLETYPE cardHandle, Int16 axNo, ref double pTrajVel);
        //多轴立即插补运动平滑停止
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateMoveStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateMoveStop(HANDLETYPE cardHandle, Int16 axNo);
        //多轴立即插补运动急停
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ImmediateMoveEStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ImmediateMoveEStop(HANDLETYPE cardHandle, Int16 axNo, double estopDec);
		
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			多轴同步运动模式接口     					~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //多轴同步运动映射建立
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetupMutiSyncSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetupMutiSyncSys(HANDLETYPE cardHandle, Int16 groupNo, Int16[] pMaskAxNo, Int16 maxAxNum);
        //获取多轴同步运动的坐标系信息
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetMutiSyncSysInfo", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetMutiSyncSysInfo(HANDLETYPE cardHandle, Int16 groupNo, Int16[] pMaskAxNo, ref Int16 pMaxAxNum);
        //删除多轴同步运动映射
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DeleteMutiSyncSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DeleteMutiSyncSys(HANDLETYPE cardHandle, Int16 groupNo);
        //多轴同步运动线段输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotion(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, short[] pVelRatio, short[] pAccRatio, double blendRatio, Int32 userID = 1);
        // 以最大速度、最大加减速度的比例参数形式进行的多轴线段输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionInRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionInRatio(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, short[] pVelRatio, short[] pAccRatio, double blendRatio, Int32 userID = 1);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionLookaheadInRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionLookaheadInRatio(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, short[] pVelRatio, short[] pAccRatio, double cornerRatio, Int32 userID = 1);
        // 以各轴的实际速度、加速度参数的形式进行的多轴线段输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionInAxVelAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionInAxVelAcc(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, double[] pTrajVel, double[] pTrajAcc, double[] pTrajdec, double blendRatio, Int32 userID = 1);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionLookaheadInAxVelAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionLookaheadInAxVelAcc(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, double[] pTrajVel, double[] pTrajAcc, double[] pTrajdec, double cornerRatio, Int32 userID = 1);
        // 以所有轴的合成速度、加速度的参数形式进行的多轴线段输入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionInSynVelAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionInSynVelAcc(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPosArray, double trajVel, double trajAcc, double trajdec, double blendRatio, Int32 userID);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionLookaheadInSynVelAcc", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionLookaheadInSynVelAcc(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPosArray, double trajVel, double trajAcc, double trajdec, double cornerRatio, Int32 userID);
        // 以各轴的速度、加减速时间的参数形式进行的多轴线段输入：accTime，decTime单位为ms
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionInAccDecTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionInAccDecTime(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, double[] pTrajVel, double accTime, double decTime, double blendRatio, Int32 userID = 1);
        // 以运动总执行时间、加减速时间的参数形式进行的多轴线段输入：runTime，accTime，decTime单位为ms
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncMotionInRunTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncMotionInRunTime(HANDLETYPE cardHandle, Int16 groupNo, double[] pEndPos, double runTime, double accTime, double decTime, double blendRatio, Int32 userID = 1);
        //多轴同步运动启停
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncStart", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncStart(HANDLETYPE cardHandle, Int16 groupNo);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncStop(HANDLETYPE cardHandle, Int16 groupNo, Int16 stopType);
        //多轴同步系统运行状态获取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetStatus(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pStatus);
        //多轴同步运动倍率读写
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncSetRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncSetRatio(HANDLETYPE cardHandle, Int16 groupNo, double ratio);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetRatio(HANDLETYPE cardHandle, Int16 groupNo, ref double pRatio);
        //多轴同步运动用户ID读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetUserID", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetUserID(HANDLETYPE cardHandle, Int16 groupNo, ref Int32 pUserID);
        //获取多轴同步运动缓存区空间余量
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetSpace(HANDLETYPE cardHandle, Int16 groupNo, ref Int32 pSpace);
        //清除多轴同步运动缓存区数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncClrData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncClrData(HANDLETYPE cardHandle, Int16 groupNo);
        //多轴同步运动故障清除
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncClrError", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncClrError(HANDLETYPE cardHandle, Int16 groupNo);
        //多轴同步运动编程模式设定
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncSetType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncSetType(HANDLETYPE cardHandle, Int16 groupNo, Int16 type);
        //多轴同步运动编程模式读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetType", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetType(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 type);
        //多轴同步运动当前合成速度读取
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetTrajVel", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetTrajVel(HANDLETYPE cardHandle, Int16 groupNo, double[] pVel);
        //获取多轴同步运动最后一段到位状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiSyncGetArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiSyncGetArrivalSts(HANDLETYPE cardHandle, Int16 groupNo, Int16[] pSts);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			捆绑PT运动模式接口     					~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //多轴绑定PT系统的创建
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetupPtPackSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetupPtPackSys(HANDLETYPE cardHandle, Int16 sysNo, Int16[] pMaskAxNo, Int16 maxAxNum);
        //运动数据点的压入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AddMotionPointPtPack", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AddMotionPointPtPack(HANDLETYPE cardHandle, Int16 sysNo, Int32[] pPos, Int16[] pType, double T, Int16 dataNum);
        //DO数据点的压入
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AddDoPointPtPack", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AddDoPointPtPack(HANDLETYPE cardHandle, Int16 sysNo, Int16 doNo, Int16 doType, Int16 doLevel);
        //多轴绑定PT系统启动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StartPtPack", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StartPtPack(HANDLETYPE cardHandle, Int16 sysNo);
        //多轴绑定PT系统手动停止
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_StopPtPack", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_StopPtPack(HANDLETYPE cardHandle, Int16 sysNo, Int16 type);
        //获取系统的剩余FIFO空间量
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPtPackRestSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPtPackRestSpace(HANDLETYPE cardHandle, Int16 sysNo, ref Int16 pSpace);
        //获取绑定PT运动状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPtPackStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPtPackStatus(HANDLETYPE cardHandle, Int16 sysNo, ref Int16 pStatus);
        //删除多轴绑定PT系统
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DeletePtPackSys", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DeletePtPackSys(HANDLETYPE cardHandle, Int16 sysNo);
        //清除FIFO中所有的数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ClrPtPackData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ClrPtPackData(HANDLETYPE cardHandle, Int16 sysNo);
        //设置位置编程模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetPtPackIncMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetPtPackIncMode(HANDLETYPE cardHandle, Int16 sysNo, Int16 incMode);
        //获取位置编程模式
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPtPackIncMode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPtPackIncMode(HANDLETYPE cardHandle, Int16 sysNo, ref Int16 pIncMode);
        //使能断流保护
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnablePtPackNoDataProtect", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnablePtPackNoDataProtect(HANDLETYPE cardHandle, Int16 sysNo, double[] pThresholdVel);
        //失效断流保护
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DisablePtPackNoDataProtect", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DisablePtPackNoDataProtect(HANDLETYPE cardHandle, Int16 sysNo);
        //获取断流保护设置的状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPtPackNoDataProtectStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPtPackNoDataProtectStatus(HANDLETYPE cardHandle, Int16 sysNo, Int16[] pEnSts, double[] pThresholdVel);
        //清除多轴绑定PT系统中的错误
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ClrPtPackError", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ClrPtPackError(HANDLETYPE cardHandle, Int16 sysNo);
        //多轴绑定PT获取系统故障
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetPtPackError", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetPtPackError(HANDLETYPE cardHandle, Int16 sysNo, ref Int16 pErr);
        //多轴绑定PT获取到位状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_PtPackGetArrivalSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_PtPackGetArrivalSts(HANDLETYPE cardHandle, Int16 sysNo, ref Int16 pSts);



        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				电子齿轮功能操作接口     				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        // 设置跟随参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearSetParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearSetParam(HANDLETYPE cardHandle, Int16 axNo, ref  TGearParam pGearParam);
        // 读取跟随参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearGetParam", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearGetParam(HANDLETYPE cardHandle, Int16 axNo, ref TGearParam pGearParam);
        // 更新主轴齿数和设置离合区
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearUpdateScale", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearUpdateScale(HANDLETYPE cardHandle, Int16 axNo, double masterScale, double slaveScale, Int32 masterDis);
        //电子齿轮模式主轴离合区设定
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearSetMasterZone", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearSetMasterZone(HANDLETYPE cardHandle, Int16 axNo, Int32 masterDis);
        //电子齿轮模式启动
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearStart", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearStart(HANDLETYPE cardHandle, Int16 axNo);
        //电子齿轮模式停止
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearStop", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearStop(HANDLETYPE cardHandle, Int16 axNo, Int16 type);
        //电子齿轮错误获取状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearGetStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearGetStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pStatus);
        //电子齿轮关系销毁
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GearDestroy", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GearDestroy(HANDLETYPE cardHandle, Int16 axNo);

        ///*==========================================================================*/
        ///*----控制卡系统操作接口                                             ---*/
        ///*==========================================================================*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				控制卡系统监控操作   				~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        //获取控制卡的规划周期和卡类型
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetHwSysPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetHwSysPara(HANDLETYPE cardHandle, ref Int16 pCycleTime, ref Int16 pHwType);
        //复位控制卡软件
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ResetSysPara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ResetSysPara(HANDLETYPE cardHandle);
        //获取控制卡的CPU负载率
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetCalcLoadRatio", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetCalcLoadRatio(HANDLETYPE cardHandle, ref double pLoadRatio);
        //获取控制卡软件版本
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetVersion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetVersion(HANDLETYPE cardHandle, Int16[] pVersion);
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetVersionEx", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetVersionEx(HANDLETYPE cardHandle, Int16[] pVersion);
        // 获取板卡资源最大数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetResMax", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetResMax(HANDLETYPE cardHandle, Int16 type, ref Int16 pResMaxValue);
        //忽略DSP检测EtherCAT断线状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEcatLinkStsIgnoreFlag", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEcatLinkStsIgnoreFlag(HANDLETYPE cardHandle, Int16 flag);
        // 获取板卡版本信息
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetIMC30G_VER", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetIMC30G_VER(HANDLETYPE cardHandle, string pathName);
        // 获取控制卡硬件版本
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetHwVer", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetHwVer(HANDLETYPE cardHandle, ref Int16 pVer);
        // 获取出厂编码，最大32字节，目前16字节字符串
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetFactoryCode", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetFactoryCode(HANDLETYPE cardHandle, string pCode);
        // 获取整个系统版本号
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetImcCardVersion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetImcCardVersion(HANDLETYPE cardHandle, UInt16 type, string pVersion);
        // 获取板卡资源(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetResCount", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetResCount(HANDLETYPE cardHandle, Int16 type, ref Int16 pResCountValue);
        //获取系统运行记录时间(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetTime(HANDLETYPE cardHandle, Int16 timeType, ref UInt32 pTime, ref UInt32 pMaxTime);
        //清除系统运行记录时间(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ClearTime", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ClearTime(HANDLETYPE cardHandle, Int16 timeType);
        //获取arm上的时钟(V1.11.1.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetArmClock", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetArmClock(HANDLETYPE cardHandle, ref UInt16 pClock1, ref UInt16 pClock2);
        //复位板卡imc_ecat系统(V1.11.2.0及以上固件)
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_CardSystemReset", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_CardSystemReset(HANDLETYPE cardHandle, Int16 type);

        ///*==========================================================================*/
        ///*----其他功能接口                                               ---*/
        ///*==========================================================================*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~				数据采集功能接口    					~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //配置数据采集的参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ConfigSamplePara", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ConfigSamplePara(HANDLETYPE cardHandle, ref TSamplePara pSamplePara);
        //配置数据采集的采集对象
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ConfigSampleData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ConfigSampleData(HANDLETYPE cardHandle, Int16 count, Int16[] pDataType, Int16[] pDataIndex);
        //使能数据采集
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ConfigSampleEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ConfigSampleEnable(HANDLETYPE cardHandle, Int16 enable);
        //获取数据采集状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSampleStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSampleStatus(HANDLETYPE cardHandle, ref Int16 pStatus, ref Int32 pLen, ref Int32 pLeakageCount);
        //获取采集的数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetSampleData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetSampleData(HANDLETYPE cardHandle, ref Int16 pPackNum, Int16[] pData);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			事件管理功能接口							~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置IO事件参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEventIO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEventIO(HANDLETYPE cardHandle, Int16 group, ref TEventIO pEventIOPara);
        //获取IO事件参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventIO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventIO(HANDLETYPE cardHandle, Int16 group, ref TEventIO pEventIOPara);
        //使能IO事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnableEventIO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnableEventIO(HANDLETYPE cardHandle, Int16 group);
        //禁止IO事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DisableEventIO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DisableEventIO(HANDLETYPE cardHandle, Int16 group);
        //获取IO事件使能状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventIOEnSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventIOEnSts(HANDLETYPE cardHandle, Int16 group, ref Int16 enSts);
        //销毁IO事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DestroyEventIO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DestroyEventIO(HANDLETYPE cardHandle, Int16 group);
        //获取IO事件管理参数是否设置状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventIOSetupSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventIOSetupSts(HANDLETYPE cardHandle, Int16 group, ref Int16 pSts);
        //设置DI触发运动事件参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEventDiMotion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEventDiMotion(HANDLETYPE cardHandle, Int16 group, ref TEventDiMotion pEventDiMotionPara);
        //获取DI触发运动事件参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventDiMotion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventDiMotion(HANDLETYPE cardHandle, Int16 group, ref TEventDiMotion pEventDiMotionPara);
        //使能DI触发运动事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnableEventDiMotion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnableEventDiMotion(HANDLETYPE cardHandle, Int16 group);
        //禁止DI触发运动事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DisableEventDiMotion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DisableEventDiMotion(HANDLETYPE cardHandle, Int16 group);
        //获取DI触发运动事件的状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventDiMotionSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventDiMotionSts(HANDLETYPE cardHandle, Int16 group, ref Int16 pSts);
        //清除DI触发运动事件的错误状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ClearEventDiMotionSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ClearEventDiMotionSts(HANDLETYPE cardHandle, Int16 group);
        //获取DI触发运动事件使能状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventDiMotionEnSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventDiMotionEnSts(HANDLETYPE cardHandle, Int16 group, ref Int16 enSts);
        //销毁DI触发运动事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DestroyEventDiMotion", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DestroyEventDiMotion(HANDLETYPE cardHandle, Int16 group);
        //获取DI触发运动事件参数是否设置状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventDiMotionSetupSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventDiMotionSetupSts(HANDLETYPE cardHandle, Int16 group, ref Int16 pSts);
        //设置比较输出事件参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEventCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEventCompareOut(HANDLETYPE cardHandle, Int16 group, ref TEventCompareOut pEventCompareOutPara);
        //获取比较输出事件参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventCompareOut(HANDLETYPE cardHandle, Int16 group, ref TEventCompareOut pEventCompareOutPara);
        //使能比较输出事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnableEventCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnableEventCompareOut(HANDLETYPE cardHandle, Int16 group);
        //禁止比较输出事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DisableEventCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DisableEventCompareOut(HANDLETYPE cardHandle, Int16 group);
        //获取比较输出事件管理使能状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventCompareOutEnSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventCompareOutEnSts(HANDLETYPE cardHandle, Int16 group, ref Int16 enSts);
        //销毁比较输出事件管理
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_DestroyEventCompareOut", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_DestroyEventCompareOut(HANDLETYPE cardHandle, Int16 group);
        //获取比较输出事件参数是否设置状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventCompareOutSetupSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventCompareOutSetupSts(HANDLETYPE cardHandle, Int16 group, ref Int16 pSts);

        //设置事件管理中的虚拟IO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetEventVirtualIOVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetEventVirtualIOVal(HANDLETYPE cardHandle, Int16 port, Int16 val);
        //获取事件管理中的虚拟IO值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventVirtualIOVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventVirtualIOVal(HANDLETYPE cardHandle, Int16 port, ref Int16 pVal);
        //获取事件管理中的虚拟IO是否已使用
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventVirtualIOUseSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventVirtualIOUseSts(HANDLETYPE cardHandle, Int16 port, ref Int16 pSts);
        //使能事件管理中的虚拟IO
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_EnableEventVirtualIO", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_EnableEventVirtualIO(HANDLETYPE cardHandle, Int16 port, Int16 en);
        //获取事件管理中的虚拟IO的使能状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetEventVirtualIOEnableSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetEventVirtualIOEnableSts(HANDLETYPE cardHandle, Int16 port, ref Int16 pSts);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			龙门功能操作							    ~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetupAxGantry", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetupAxGantry(HANDLETYPE cardHandle, Int16 groupNo, Int16 masterAxNo, Int16 slaveAxNo, Int32 errorLmt);

        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AbortAxGantry", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AbortAxGantry(HANDLETYPE cardHandle, Int16 groupNo);

        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxGantrySts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxGantrySts(HANDLETYPE cardHandle, Int16 groupNo, ref Int16 pMasterAxNo, ref Int16 pSlaveAxNo, ref Int32 pErrorLmt, ref Int16 pSts);

        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_ClrAxGantrySts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_ClrAxGantrySts(HANDLETYPE cardHandle, Int16 groupNo);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			螺距误差补偿功能操作					~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //初始化螺距补偿表
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxSetScrewCompTable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxSetScrewCompTable(HANDLETYPE cardHandle, Int16 axNo, Int16 dir, Int16 paraNum, Int32[] pCompValArray);
        //获取螺距补偿表
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxGetScrewCompTable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxGetScrewCompTable(HANDLETYPE cardHandle, Int16 axNo, Int16 dir, ref Int16 paraNum, Int32[] pCompValArray);
        //打开螺距补偿功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxEnableScrewComp", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxEnableScrewComp(HANDLETYPE cardHandle, Int16 axNo, Int32 posStartPos, Int32 posEndPos, Int16 pointNum);
        //关闭螺距补偿功能
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxDisableScrewComp", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxDisableScrewComp(HANDLETYPE cardHandle, Int16 axNo);
        //获取补偿状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxGetScrewCompStatus", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxGetScrewCompStatus(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pSts);
        //获取补偿位置
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_AxGetScrewCompValue", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_AxGetScrewCompValue(HANDLETYPE cardHandle, Int16 axNo, ref Int32 pCompVal);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			非标510功能接口							~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        //设置SV510压合控制字-PDO对象为604D
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxPressFitCtrlWord", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxPressFitCtrlWord(HANDLETYPE cardHandle, Int16 axNo, UInt16 ctrlword);
        //设置目标压力值 对应伺服功能码为H10-00
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_SetAxPressTgtVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_SetAxPressTgtVal(HANDLETYPE cardHandle, Int16 axNo, Int16 pressVal);
        //获取SV510伺服压合状态字-PDO对象为604E
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPressFitSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPressFitSts(HANDLETYPE cardHandle, Int16 axNo, ref UInt16 pSts);
        //获取伺服模拟量输入接口的AI1 AI2采样电压值
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxAIInputVal", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxAIInputVal(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pA1Volt, ref Int16 pA2Volt);
        //获取反馈压力值-PDO对象为6054
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_GetAxPressFeedback", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_GetAxPressFeedback(HANDLETYPE cardHandle, Int16 axNo, ref Int16 pPressVal);

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        /*~~~			非标端子板功能接口						~~~*/
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        // 配置多轴位置比较输出功能参数
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiAxCmpCfg", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiAxCmpCfg(HANDLETYPE cardHandle, Int16[] pCompSrcArray, Int16[] pPulseWidth, UInt32 errorValue, Int16 mutiSrcType);
        // 写入多轴位置比较数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiAxCmpDataIn", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiAxCmpDataIn(HANDLETYPE cardHandle, Int32[] pCmpData, Int16 portMask, ref Int16 pSpace);
        // 清空多轴位置比较数据
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiAxCmpClrData", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiAxCmpClrData(HANDLETYPE cardHandle);
        // 获取多轴位置比较剩余空间
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiAxCmpGetSpace", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiAxCmpGetSpace(HANDLETYPE cardHandle, ref Int16 pBufSpace);
        // 启动多轴位置比较输出
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiAxCmpEnable", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiAxCmpEnable(HANDLETYPE cardHandle, Int16 enable);
        // 获取多轴位置比较状态
        [DllImport(EtherCATConfigApiDllName, EntryPoint = "IMC_MutiAxCmpGetSts", ExactSpelling = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 IMC_MutiAxCmpGetSts(HANDLETYPE cardHandle, ref Int16 pCmpSts, ref Int16 pCmpCount, ref Int16 pCmpIndex);
  
    }
}
