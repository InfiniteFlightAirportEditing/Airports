Editing airports requires a bit of setup, but can be very simple once you get started.

Requirements
================
  - A GitHub account (let Cameron know your username)
  - A copy of X-Plane (you can download the demo from here www.x-plane.com/downloads/landing/)
  - A copy of X-Plane's WED (developer.x-plane.com/tools/worldeditor/)
  - (recommended) GitHub app (windows.github.com or mac.github.com)
  
  
Setting up X-Plane
================
Setting up X-Plane is very simple - simply download the demo (or install the cd/steam copy), open up your copy of WED and point it to your X-Plane directory.


Setting up Git (made simpler with SourceTree)
================
Download the GitHub app from the link above and login with your username and details.

Then, tap the "+" in the top left corner, select the "Clone" tab and select the /infiniteflight repo (make sure Cameron has your username first).

Select a location to save, and tap "Clone".

Editing in WED
================
(full manual here - http://developer.x-plane.com/manuals/wed/)

Open up WED, and create a new package (name it the name of the airport you're going to edit).

You'll want an existing copy of the airport so you do not have to completely create it - there are two ways of doing this:

  1) Finding freeware scenery on websites such as X-Plane.org
  2) Getting the X-Plane source data file (http://dev.x-plane.com/update/data/AptNav201310XP1000.zip)
  
Once you download one of these options, you'll need to find a file named "apt.dat" inside the folder. If you downloaded from existing scenery (option 1), it will be found in a folder named "Earth nav data".

In WED, go to File - Import from apt.dat.
Select an apt.dat that you previously downloaded.
A list of airports in that file will list - select the one you want to edit, and click OK.

Editing is fairly straightforward, there are tutorials online if you need help with that.

Exporting from WED
================
Save the file (ctrl + s), then go to File - Export as apt.dat.
Save the file to the folder which you cloned in the GitHub app, under a new folder named by the ICAO of the airfield.

Updating the repo
================
Open the GitHub app and select the infiniteflight repo.

Select all the changed files under the "Uncommited Changes" section, and add a brief message stating the changes you have made.

Tap "Commit"; once Committing has finished, tap "Sync" in the top-right corner.


---
This should be all you need to get started. Message me if anything isn't working :)
