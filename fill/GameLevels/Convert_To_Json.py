import os, sys
import re
################################################################################

DEF_lineColor = "\"lineColor\" : {\"r\": 0.392,\"g\": 0.047, \"b\": 0.012, \"a\": 1.0}"
DEF_backgroundColor = "\"backgroundColor\" : {\"r\": 0.392,\"g\": 0.047, \"b\": 0.012, \"a\": 1.0}"
DEF_guardBasicColor = "\"guardBasicColor\" : {\"r\": 0.492,\"g\": 0.147, \"b\": 0.112, \"a\": 1.0}"
DEF_guardSelectedColor = "\"guardSelectedColor\" : {\"r\": 0.592,\"g\": 0.247, \"b\": 0.212, \"a\": 1.0}"
DEF_VGColor = "\"vgColor\" : {\"r\": 0.692,\"g\": 0.347, \"b\": 0.312, \"a\": 0.2}"
################################################################################
def printHelp(argv):
    print "./" + argv[0] + " FOLDER_OF_RAW_MAP_DATA"
################################################################################
def Main():    
    if(len(sys.argv) <= 1):
        printHelp(sys.argv)
        return
    
    file_paths = get_filepaths(sys.argv[1])
#    print "1. Valid files inside the path: "
    ret, outerFilename, holeFiles = validateFileNames(file_paths)
    print "Return: " + str(ret)
    
#    print "2. Print prefix"
    printPrefix("asdf", 10)
    
#    print "3. Get outer vertices"
    printOuter(outerFilename)
    
#    print "4. Get hole vertices"
    printHoles(holeFiles, outerFilename)
    
#    print "5. Print postfix"
    printPostfix("asdf")
    
################################################################################
def get_filepaths(directory):
    """
    This function will generate the file names in a directory 
    tree by walking the tree either top-down or bottom-up. For each 
    directory in the tree rooted at directory top (including top itself), 
    it yields a 3-tuple (dirpath, dirnames, filenames).
    """
    file_paths = []  # List which will store all of the full filepaths.

    # Walk the tree.
    for root, directories, files in os.walk(directory):
        for filename in files:
            # Join the two strings in order to form the full filepath.
            filepath = os.path.join(root, filename)
            file_paths.append(filepath)  # Add it to the list.

    return file_paths  # Self-explanatory.

################################################################################
def readCoord(filename):
#    print "*readCoord(" + filename + ")"
    toRet = []
    coordLine = re.compile("(-)?[\\d]{1,}[.]?[\\d]{0,}\t(-)?[\\d]{1,}[.]?[\\d]{0,}")
    
    with open(filename) as f:
        for line in f:
            m = coordLine.match(line)
            if m:
                coords = re.findall("(-?[\\d]{1,}\.?[\\d]{0,})", line)
                toRet.append("{\"x\": " + str(coords[0]) + ", \"y\": " + str(coords[1]) + "}")
                
    return toRet
            
################################################################################
def validateFileNames(file_paths):
    outerFound = False
    outerFilename = ""
    holeFiles = []
    
    outer = re.compile(".*_\\d\\d_Outer.txt")
    hole = re.compile(".*_\\d\\d_Hole\\d\\d.txt")
    
    for i in range(0, len(file_paths)):
        print "*Validate filename: " + file_paths[i]

        holeM = hole.match(file_paths[i])
        if(holeM): # if file is hole
            ints = re.findall('\\d\\d', file_paths[i])
            
            holeFiles.append(file_paths[i])
            # check if the level number matches the intention
#            for j in range(0, len(ints)):
#                if(compareLevel == None):
#                    compareLevel = ints[j]
                    
#                if(compareLevel != ints[j]):
#                    print "*Does not match current level setting, aborting..."
#                    return False
            
        else: # if file is not hole check if it is outer
            outerM = outer.match(file_paths[i])
            
            if(outerM):
                ints = re.findall('\\d\\d', file_paths[i])
            
#                # check if the level number matches the intention
#                if(compareLevel == None):
#                    compareLevel = ints[0]
#
#                if(compareLevel != ints[0]):
#                    print "*Does not match current level setting, aborting..."
#                    return False

                print "*Outer found!"
                outerFound = True
                outerFilename = file_paths[i]
                
            else:
                return False
        
    return outerFound, outerFilename, holeFiles
################################################################################
def printPrefix(name, stage_id):
    print "{"
    print "\t\"name\" : \"" + name + "\","
    print "\t\"id\" : \"" + str(stage_id) + "\","

def printOuter(outerFilename):
    coords = readCoord(outerFilename)
    print "\t\"outerVertices\" : ["
    for i in range(0, len(coords) - 1):
        print "\t\t" + coords[i] + ","
    print "\t\t" + coords[-1]
    print "\t],"
    
def printHoles(file_paths, outerFilename):
    print "\t\"holes\" : ["
    for i in range(0, len(file_paths) - 1):
        filename = file_paths[i]
        if(filename != outerFilename): # if it is a hole
            coords = readCoord(filename)
            print "\t\t{ \"innerVertices\" : ["
            for i in range(0, len(coords) - 1):
                print "\t\t\t" + coords[i] + ","
            print "\t\t\t" + coords[-1]
            print "\t\t]},"
            
    filename = file_paths[-1]
    if(filename != outerFilename): # if it is a hole
        coords = readCoord(filename)
        print "\t\t{ \"innerVertices\" : ["
        for i in range(0, len(coords) - 1):
            print "\t\t\t" + coords[i] + ","
        print "\t\t\t" + coords[-1]
        print "\t\t]"
    print "\t}],"
    
def printPostfix(filePath):
    print "\t" + DEF_lineColor + ","
    print "\t" + DEF_backgroundColor + ","
    print "\t" + DEF_guardBasicColor + ","
    print "\t" +  DEF_guardSelectedColor + ","
    print "\t" +  DEF_VGColor + ","
    print "\t\"filePath\" : \"" + filePath + "\""
    print "}"
    
Main()
    
