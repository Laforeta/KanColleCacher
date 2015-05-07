KanColleCacher
====================


What is this?
-----------------------

KanColleCacher.dll is a plug-in for KanColleViewer (KCV) to prove persistent and easily manageable cache files. 

The main benefits of using this plug-in is reduced data usage and faster game loading as cache files are no longer randomly flushed by the browser.

In addition, this tool can also be used to change the graphics and sound as they appear in the game. Please read the instructions thoroughly and use this function responsibly. 


Version Support
-----------------------
* The latest test release is v2.1.0
* Tested to be fuly compatible with v3.3 through to v3.6 of the main KCV branch, as well as yuyuvn's KCV v3.7 build r544.
* It does NOT appear to function properly with pre-3.0 versions of KancolleViewer, avoid this plugin if you are still on an older version of KCV. 
* Compatibility with future versions of KancolleViewer is not guaranteed as the underlying code base may change. 


License
--------------------
* Released under MIT License


Installation
--------------------
* Copy KancolleCacher.dll to the 'Plugins' folder of your KCV installation
* (Optional) Right click on KancolleCacher.dll, choose 'Properties', click 'Unblock' and "OK"
* Launch KanColleViewer
* The settings for this plugin can be found under the 'Tools' tab

How to use
--------------------
W.I.P.


Known Issues
--------------------
1. Changes made to local filed managed by this plugin is not applied to the game until the cache is manually wiped in KCV. This is not a bug but as design constraint as the browser contain in KCV is fully oblivious of packet interception and will try to maintain its own cache. 

2. In rare cases, the plugin may cause KCV to freeze when a new file is being written to the disk. The cause of this is not yet defined, however it is very infrequent and easily resolved by restarting KCV.


Work to do
--------------------
1. Add more documentation to allow troubleshooting from user's end and encourage further developement. 

2. Implement file integrity checks as the current file verification mechanism only checks the Last-Modified time. 