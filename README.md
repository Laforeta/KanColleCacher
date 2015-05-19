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
* Tested to be fully compatible with v3.3 through to v3.6 of the main KCV branch, as well as yuyuvn's KCV v3.7 build r544.
* Compatibility with future versions of KancolleViewer is not guaranteed as the underlying code base may change. 


License
--------------------
* Released under MIT License


Installation
--------------------
* Copy KancolleCacher.dll to the 'Plugins' folder of your KCV installation
* (Optional but highloy recommended) Right click on KancolleCacher.dll, choose 'Properties', click 'Unblock' and "OK"
* Launch KanColleViewer
* The settings for this plugin can be found under the 'Tools' tab

Basic Operations
--------------------


Cache File Structure
--------------------
/kcs/portmain.swf
/kcs/resources/
/kcs/scenes/
/kcs/sound/


Modding: Character Sprites
--------------------


Modding: Sound
--------------------


Modding: Welcome Message
--------------------


Troubleshooting and Known Issues
--------------------
1. Changes made to local files managed by this plugin is not applied to the game until the cache is manually wiped in KCV. This is not a bug but as design constraint as the browser contained in KCV is oblivious of packet interception and will try to maintain its own cache. 

2. While the plugin attempts to keep every file in cache up to date, cache files are sometimes received in incomplete or corrupted state due to transmission errors. If a particular actions such as sortie to a certain node or selecting a certain ship consistently causes the game to error out, it is possible that some files may have become corrupted. This is unlikely to have long-term consequences as it could happen without this tool. To remedy this, manually delete all cached files under /kcs/ and they will be automatically re-downloaded by the next restart. An improved file verification scheme is planned for a future release to relieve the need for doing this. 

3. In rare cases, the plugin may cause KCV to freeze when ateempting to overwrite a file that needs to be updated. The cause of this is not yet 100% clear, however it is very infrequent and easily resolved by restarting KCV.

Work to do
--------------------
1. Add more documentation to allow troubleshooting from user's end and encourage further developement. 

2. Implement file integrity checks as the current file verification mechanism only checks the Last-Modified time. 