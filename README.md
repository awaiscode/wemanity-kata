# wemanity-kata
This is a kata to demonstrate some quick coding samples

"Service Multi Process"
Application overview: This is a windows service application that can be added in windows services.
Once service is started it can launch multiple instances of any application depending on the requirements.

Sample Usage: 
I have a requirement that in our web portal users can make requests to generate some reports which depends on another 
windows report generation application. This windows application is quite heavy and use many resources so we want to launch it only once there
are some reports are requested from the web portal. We have thousands of such requests on daily basis.

Application overview/features: 
"Service Multi Process" is responsible to check the queue and launch the report application once needed.
It can launch multiple instances depending on the queue right now maximum allowed instaces are four and can be increased or decreased.
If a windows report application is taking too much long time and is running for more than three hours the service application should kill
windows application automatically. There are further usage scenarios that are not described here.

Application Testing:
As its not a very large service the unit tests are not added for testing and fixes I am logging into a text log file named as 'Trace' 
to monitor the service and keep track of its crashes and bugs.



