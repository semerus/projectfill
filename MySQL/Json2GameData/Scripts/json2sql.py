JSON_FOLDER="./MapJson/"
IMG_FOLDER="C:\\mp_img\\"
mecca=[8, 1, "Mecca.json", "Mecca.png"]
empire=[9, 1, "Empire_State_Building.json", "Empire.png"]

# 기존 secure_file_prive
# secure_file_priv = "C:\ProgramData\MySQL\MySQL Server 8.0\Uploads\"

maps=[]
maps.append(mecca)
maps.append(empire)
for m in maps:
    jsonF = open(JSON_FOLDER + m[2], 'r')
    json = jsonF.read().replace('"', '\\"').replace("\t", '').replace("\r\n", '')
    jsonF.close()

    print("INSERT INTO GameData VALUES(%d, %d, \'%s\', LOAD_FILE(\'%s\'));" % (m[0], m[1], json, IMG_FOLDER + m[3]))
