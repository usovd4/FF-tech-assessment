# FourteenFish - Full Stack Technical Assessment Reflection

## Submission of the solution

First of all, to get this out of the way - I've read instructions for the submission once I was done with the development, therefore I've done exactly what I was asked NOT to do and created a 'single monolithic push'. We're off to a great start.

## Reflection of the assessment

I've found the task fairly challenging due to the fact that I've neither written a .Net Core application nor interacted with MySQL in the last 5 years. I've also had to confront the fact that I've become quite reliant on working with DevExpress and once the tools were not available to me anymore even the simpler tasks became much more challenging. Overall, I would say that the assessment is 'hard' rather that despite understanding what I need to do in principle I still had to do a lot of research to actually create a working solution.

## Packages used

Newtonsoft.JSON - to deserialize the JSON data provided.
Entity Framework - to build the data access layer.

I've also included the updated sql seed data to create and populate additional database tables (db connection string is removed from environmental variables as it was pointing to my local db and would be useless).

## Possible future implementations

While the application is working and should do everything that it has been tasked to do and clear the acceptance criteria, it is definitely a very barebones application. In this section I'll briefly talk about what could be done if I had more time to work on the project.

### Front-end:

Front-end is extremely basic, since I focused on functionality I didn't really have time to tinker with the looks of the application - so that is definitely something that can be improved. The views and navigation is also very basic - there are no partial views / popups etc. everything leads to a new page.
The controls could be improved further - tables could benefit from the addition of filtering, search functionality, sorting and pagination. Mapping of the specialties to the doctors is done using checkboxes - this may not be a good solution depending on the circumstances (for expample if there is a great number of specialties in the system), but for the purpose of the exercise it worked well enough. JSON file upload is on the home page right next to the people table - this works well as you can see the results of the upload right away, but is not an ideal place for it to be. If I had more time I would put it on the admin page next to an admin version of the People table that would allow you to add/edit/remove the users.

### Back-end:

I used entity framework to interact with the database and all of the methods that I have written utilize it, but the ones that were already there I have left untouched so this would be something to fix in the future. Regarding the JSON parsing I had to make some assumptions. I have noticed that not all of the data in the json file is well structured - names can be written in all uppercase, some users have multiple addresses and some are missing the address property entirely. The assumptions that I made during the development are:
1. I expect users to have proper firstName, lastName and GMC parameters and values. I am not dealing with uppercase names for now.
2. I will not throw an exception if some of the objects are missing an address, instead I will try to get as much info out of the object as possible and will leave other fields blank
3. In case of multiple addresses, since the front end of the application does not support that I shall take the first address as the 'correct' one.


### SQL

Aside from some initial setup troubleshooting I had to do since I am used to MsSQL instead of MySQL I don't have much to say. I've created 2 additional tables - one to store specialities and the other to map doctors to specialities. I've included the additions in the sql seed.

