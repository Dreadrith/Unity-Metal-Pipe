# Listed-VPM-Template
This repo was made real quick for personal use but feel free to use it too.<br>
This is the essentials to get a repo package listed in the page you make from [the VPM listing template](https://github.com/vrchat-community/template-package-listing) <br><br>

## Instructions
1. Use this template to make your own repository. Can be private but public later.<br>
2. Replace the appropriate/example things in every file:
	- .git/release.yml - Just lines 7 to 11  
	- package.json, CHANGELOG.md, LICENSE.md  
	- Assembly Definitions file names AND 'Name' (click on it)  
	- Documentation~/com.dreadscripts.example (File name too!)  

3. Delete script folders, documentation, CHANGELOG and LICENSE if they're unused.<br>
4. If the Repo isn't public, make it public in settings.<br>
5. Go to Actions > Build Release > Run Workflow > Run Workflow.
6. Go to your listing repository and add the repo you made from this template to 'source.json' under 'githubRepos'.<br>
7. On your listing repository, also go to Actions > Build Repo Listing > Run Workflow > Run Workflow. This should update the listing.
8. Done!<br><br>


## Note
Thanks to Step 5, lovely github actions, and code from VRC's Template, your package should be automatically built in a .unitypackage and .zip with package.json in releases with your defined version as the tag.<br>
Any time you make changes to your package's files, step 5 should automatically run again on its own!<br>
You may have to do Step 7 each time though.


## WARNING
For the .unitypackage to build properly with Github actions, you need to include the .meta files for the package.<br>
This is slightly customized to fit my liking with the naming and versioning so who knows if it'll bite in the butt me later.<br>
[Here's my VPM Listing!](https://vpm.dreadscripts.com/)
