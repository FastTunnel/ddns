# ddns
自建ddns服务，将域名映射到内网计算机，可通过域名访问内网计算机。
定时查询域名解析的A记录，宽带的ip改变后自动更新A记录的ip值

## 用途
将域名动态解析到家庭宽带，当家庭宽带公网ip变化时，域名自动解析到最新的ip地址，实现通过域名永远都能访问到家庭宽带的路由器，路由器中设置转发配置，可将请求指向内网的指定设备

## 快速开始
1.修改配置文件`appsettings.json`
```
  "DDNS": {
    "openPlat": "aliyun",         // 目前仅实现了aliyun的接口
    "accessKey": "accessKey",     // 开放平台申请的accessKey
    "accessSecret": "accessSecret", // accessKey对应的accessSecret
    "domain": "test.com"            // 定时更新的 顶级域名
  }
```
2.运行程序

## 支持的域名解析服务商
- [x] 阿里云
- [ ] ...

## 原理介绍
定时查询内网电脑所有的宽带的公网ip，通过开放接口查询域名解析记录解析的ip地址，对比两个值，如果不同，通过接口更新域名解析记录的值。

## LICENSE
MIT
