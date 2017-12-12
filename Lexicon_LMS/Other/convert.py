f = open("schema.csv","r")
# FORMAT:
# datum; dag; modul; förmiddag; eftermiddag

activities = []
for line in f:
	a = line.split(sep=";")
	activities.append([a[0].replace('-',',') + ",08,30,00", a[0].replace('-',',') + ",12,00,00", "Mod"+a[2][:3], a[3]])
	activities.append([a[0].replace('-',',') + ",13,00,00", a[0].replace('-',',') + ",17,00,00", "Mod"+a[2][:3], a[4]])

activities = sorted(activities, key=lambda x: (x[2], x[3], x[0]))
	
prev = activities[0]
squashed = []
for a in activities:
	if a[2] == prev[2] and a[3] == prev[3]:
		prev[1] = a[1] # copy end datetime
	else:
		squashed.append(prev)
		prev = a

squashed = sorted(squashed, key=lambda x: (x[0]))

for a in squashed:
	type = 4
	if "övn" in a[3].lower():
		type = 3
	if "frl" in a[3].lower():
		type = 1
	if "e-l" in a[3].lower():
		type = 2	
	print("new Activity {{ ModuleId = {2}, Name=\"{3}\", StartDate = new DateTime({0}), EndDate = new DateTime({1}), Description = \"\", ActivityTypeId = {4} }},".format(*a, type))
	
		
	

	


    