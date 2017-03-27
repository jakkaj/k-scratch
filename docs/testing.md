# Remotely testing function 

K-scratch has the ability to run Azure Functions just as if you clicked the "run" button in the Azure Portal. 

All you need to do is pass in the function admin key (different to the .PublishSettings file) so the app can perform admin operations on your behalf.

To grab the admin key - go in to any of your Functions in the portal (in the Develop sub area) and click on Keys over on the right. Use the _master key. 

<img src="https://cloud.githubusercontent.com/assets/5225782/24336712/9d8df6da-12df-11e7-9f05-e8a1008500ba.JPG" width="500px"/>


Pass this key in with the -k option. You will also need to use the -l option to connect and stream the log or the app will instantly quit. 

Once it's running it will list each of your functions with a number next to them. Press that number to run the function.

**Press "r" to refresh the list incase you change your function data (or update the test data).**

K-scratch automatically uses the test data you've used in the test area of the Function editor in the Azure portal. 

If you're testing with Blob tiggers, make sure the file really exists (and in the test thing put [container]/path/to/file.txt etc.)

It will work with HttpTriggers (including querystring and headers configured in the portal) as well as table storage, blog storage and Service bus + the rest!


<img src="https://cloud.githubusercontent.com/assets/5225782/24336642/9bf814d2-12de-11e7-998e-a97525334315.gif" width="600px"/>

