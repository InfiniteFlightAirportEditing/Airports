Editing airports requires a bit of setup, but can be very simple once you get started.

Requirements
================
  - A GitHub account (let Cameron know your username)
  - A copy of X-Plane (you can download the demo from here www.x-plane.com/downloads/landing/)
  - A copy of X-Plane's WED (developer.x-plane.com/tools/worldeditor/)
  - (recommended) SourceTree for managing git stuff (sourcetree.com)
  
  
Setting up X-Plane
================
Setting up X-Plane is very simple - simply download the demo (or install the cd/steam copy), open up your copy of WED and point it to your X-Plane directory.


Setting up Git (made simpler with SourceTree)
================
Download SourceTree from the link above and install it (follow the instructions, you may need to install Git but ST will cover that for you).

In the Bookmarks window that appears, tap "New Repository", then "Clone from URL".

Set the source URL to "https://github.com/carmichaelalonso/infiniteflight.git"
You can choose the destination path and name - make it somewhere where you can save files to.

Tap Clone, and SourceTree will make a copy of the airports folder to your PC, where you can edit existing airports or add new ones.

Once Cloning has completed, a new window will appear. In the top right corner, tap on Settings, where a section named "Remotes" should appear.

Create a new remote, name it "GitHub", set the url/path to "https://github.com/carmichaelalonso/infiniteflight.git".
Under "Optional Extended Integration", set the Host Type to GitHub and login with your username and password for GitHub.

Try making a "Pull" by tapping the "Pull" button on the top bar. You may need to click "Refresh" next to "Remote branch to pull" first (it should say "master"), and then tap OK.
If it works without errors, you're ready to go!

One more thing though... (thanks Nik for pointing it out)

Open the Command Prompt (open the start menu and enter "cmd" - it should appear) and enter two commands:

       git config --global user.name "YOUR NAME HERE"
        git config --global user.email "YOUR EMAIL HERE"

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
Save the file to the folder which you cloned in SourceTree, under a new folder named by the ICAO of the airfield.

Updating the repo
================
Open SourceTree again, and you should see a list of "Staged" and "Unstaged Files".
The airport you just export should be listed under the "Unstaged files" sections. Tap the checkbox next to it to add it to staged files.

Once all files are in "Staged Files", tap on "Commit" in the top bar. Enter a brief message explaining the changes you made (e.g. "added KTMK").
Select "Immediately push to GitHub/master", then tap on "Commit".


---
This should be all you need to get started. Message me if anything isn't working :)
