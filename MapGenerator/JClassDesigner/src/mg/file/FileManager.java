package mg.file;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import javax.json.Json;
import javax.json.JsonArray;
import javax.json.JsonArrayBuilder;
import javax.json.JsonObject;
import javax.json.JsonReader;
import javax.json.JsonWriter;
import javax.json.JsonWriterFactory;
import javax.json.stream.JsonGenerator;
import mg.EnumContainer.LINE_TYPE;
import mg.data.DataManager;
import mg.data.info.RenderInfo;
import mg.data.obj.DesignObj;
import mg.data.obj.LineSeg;
import mg.data.obj.Point;
import mg.data.obj.PolygonObj;
import saf.components.AppDataComponent;
import saf.components.AppFileComponent;

/**
 * This class serves as the file management component for this application,
 * providing all I/O services.
 *
 * @author Richard McKenna
 * @author Yun Joon Soh
 * @version 1.0
 */
public class FileManager implements AppFileComponent {

    /**
     * This method is for saving user work, which in the case of this
     * application means the data that constitutes the page DOM.
     *
     * @param data The data management component for this application.
     *
     * @param filePath Path (including file name/extension) to where to save the
     * data to.
     *
     * @throws IOException Thrown should there be an error writing out data to
     * the file.
     */
    private static final String JSON_DOBJ_LIST = "dobj_list";

    @Override
    public void saveData(AppDataComponent data, String filePath) throws IOException {
        StringWriter sw = new StringWriter();

        // THEN PUT IT ALL TOGETHER IN A JsonObject
        JsonObject dataManagerJSO = Json.createObjectBuilder()
                .add(JSON_DOBJ_LIST, DataManager.getDObjListInJson())
                .build();

        // AND NOW OUTPUT IT TO A JSON FILE WITH PRETTY PRINTING
        Map<String, Object> properties = new HashMap<>(1);
        properties.put(JsonGenerator.PRETTY_PRINTING, true);
        JsonWriterFactory writerFactory = Json.createWriterFactory(properties);
        try (JsonWriter jsonWriter = writerFactory.createWriter(sw)) {
            jsonWriter.writeObject(dataManagerJSO);
        }

        // INIT THE WRITER
        OutputStream os = new FileOutputStream(filePath);
        JsonWriter jsonFileWriter = Json.createWriter(os);
        jsonFileWriter.writeObject(dataManagerJSO);
        String prettyPrinted = sw.toString();
        try (PrintWriter pw = new PrintWriter(filePath)) {
            pw.write(prettyPrinted);
        }
    }

    // HELPER METHOD FOR SAVING DATA TO A JSON FORMAT
    private static final String JSON_INFO_RI = "info_ri";
    private static final String JSON_INFO_BOX = "info_box";
    private static final String JSON_ARRAY_INFO_VAR = "info_array_var";
    private static final String JSON_ARRAY_INFO_MET = "info_array_met";
    private static final String JSON_ARRAY_INFO_EP = "info_array_ep";
    private static final String JSON_DESIGN_OBJECT_TYPE = "design_object_type";
    private static final String JSON_BOX = "box";
    private static final String JSON_BOX_ID = "box_id";

    private static final String JSON_LINE_SEG = "line_seg";
    private static final String JSON_INFO_LINE_TYPE = "line_type";
    private static final String JSON_INFO_LINE_FROM_ID = "line_start_box_id";
    private static final String JSON_INFO_LINE_TO_ID = "line_end_box_id";
    private static final String JSON_INFO_LINE_OTHERPOINTS = "line_otherpoints";

    private static final String JSON_POINT = "point";
    private static final String JSON_ENDPOINT = "endpoint";
    private static final String JSON_INFO_POINTTYPE = "point_type";
    private static final String JSON_INFO_POINT_PARENT_RI = "point_parent_ri";
    private static final String JSON_INFO_LINE_START_RI = "endpoint_start_ri";
    private static final String JSON_INFO_LINE_END_RI = "endpoint_end_ri";
    private static final String JSON_INFO_ENDPOINT_OTHER_SIDE_BOX_NAME = "endpoint_other_side_box_name";

    private static final String JSON_POLYGON = "polygon";
    private static final String JSON_POLYGON_POINTS = "polygon_points";

    public static JsonObject makeDesignObjJsonObject(DesignObj dObj) {
        JsonObject jso = null;
        if (dObj instanceof LineSeg) {
//            JsonArrayBuilder otherPointArray = Json.createArrayBuilder();
//            for (Point p : ((LineSeg) dObj).getOtherPoints()) {
//                otherPointArray.add(p.toJsonObject());
//            }
//            JsonArray jA = otherPointArray.build();

            jso = Json.createObjectBuilder()
                    .add(JSON_DESIGN_OBJECT_TYPE, JSON_LINE_SEG)
                    .add(JSON_INFO_RI, dObj.getRenderInfo().toJsonObject())
                    .add(JSON_INFO_LINE_TYPE, ((LineSeg) dObj).getLineType().toString())
                    .add(JSON_INFO_LINE_START_RI, ((LineSeg) dObj).getStart().getRenderInfo().toJsonObject())
                    .add(JSON_INFO_LINE_END_RI, ((LineSeg) dObj).getEnd().getRenderInfo().toJsonObject())
                    //                    .add(JSON_INFO_LINE_OTHERPOINTS, jA)
                    .build();

        } else if (dObj instanceof Point) {

            jso = Json.createObjectBuilder()
                    .add(JSON_DESIGN_OBJECT_TYPE, JSON_POINT)
                    .add(JSON_INFO_RI, dObj.getRenderInfo().toJsonObject())
                    .build();

        } else if (dObj instanceof PolygonObj) {
            PolygonObj pObj = (PolygonObj) dObj;

            JsonArrayBuilder otherPointArray = Json.createArrayBuilder();
            for (Point p : pObj.getPoints()) {
                otherPointArray.add(p.toJsonObject());
            }
            JsonArray jA = otherPointArray.build();

            jso = Json.createObjectBuilder()
                    .add(JSON_DESIGN_OBJECT_TYPE, JSON_POLYGON)
                    .add(JSON_POLYGON_POINTS, jA)
                    .build();
        } else {
            throw new IllegalArgumentException();
        }

        return jso;
    }

    private static final String JSON_INFO_RI_POS_X = "info_ri_pos_x";
    private static final String JSON_INFO_RI_POS_Y = "info_ri_pos_y";
    private static final String JSON_INFO_RI_L = "info_ri_length";
    private static final String JSON_INFO_RI_H = "info_ri_height";

    public static JsonObject makeRenderInfoObject(RenderInfo ri) {
        JsonObject jso;
        jso = Json.createObjectBuilder()
                .add(JSON_INFO_RI_POS_X, ri.getPosX())
                .add(JSON_INFO_RI_POS_Y, ri.getPosY())
                .add(JSON_INFO_RI_L, ri.getLength())
                .add(JSON_INFO_RI_H, ri.getHeight())
                .build();
        return jso;
    }

    /**
     * This method loads data from a JSON formatted file into the data
     * management component and then forces the updating of the workspace such
     * that the user may edit the data.
     *
     * @param data Data management component where we'll load the file into.
     *
     * @param filePath Path (including file name/extension) to where to load the
     * data from.
     *
     * @throws IOException Thrown should there be an error reading in data from
     * the file.
     */
    @Override
    public void loadData(AppDataComponent data, String filePath) throws IOException {
        // CLEAR THE OLD DATA OUT
        DataManager dataManager = (DataManager) data;
        dataManager.reset();

        // LOAD THE JSON FILE WITH ALL THE DATA
        JsonObject json = loadJSONFile(filePath);

        // LOAD THE ARRAY LIST OF DESIGN OBJECTS
        JsonArray jsonDObjListArray = json.getJsonArray(JSON_DOBJ_LIST);
        loadDObjList(jsonDObjListArray);
    }

    // HELPER METHOD FOR LOADING DATA FROM A JSON FORMAT
    private JsonObject loadJSONFile(String jsonFilePath) throws IOException {
        JsonObject json;
        try (InputStream is = new FileInputStream(jsonFilePath); JsonReader jsonReader = Json.createReader(is)) {
            json = jsonReader.readObject();
        }
        return json;
    }

    // HELPER METHOD FOR LOADING DATA FROM A JSON FORMAT
    private void loadDObjList(JsonArray jA) {
        ArrayList<DesignObj> dObjList = new ArrayList<>();
        DataManager.setdObjList(dObjList);

        // FIRST UPDATE THE ROOT
        for (int i = 0; i < jA.size(); i++) {
            JsonObject jObj = jA.getJsonObject(i);
            DesignObj oneData = loadDesignObj(jObj);
            if(oneData != null){
                System.out.println("oneData is not null");
            } else {
                System.out.println("oneData is null");
            }
            
            assert (oneData != null);
            dObjList.add(oneData);
            System.out.println("add to dObjList done");
        }
        
        System.out.println("Update DesignObj done");
    }

    private DesignObj loadDesignObj(JsonObject data) {
        DesignObj ret = null;

//        RenderInfo ri = loadRenderInfo(data.getJsonObject(JSON_INFO_RI));
        String identifier = data.getString(JSON_DESIGN_OBJECT_TYPE);
        switch (identifier) {
            case JSON_POINT:
                System.out.println("loadDesignObj : JSON_POINT");
                ret = loadPoint(data);
                
                break;
            case JSON_POLYGON:
                System.out.println("loadDesignObj: JSON_POLYGON");
                ret = new PolygonObj();

                JsonArray jA2 = data.getJsonArray(JSON_POLYGON_POINTS);

                for (int i = 0; i < jA2.size(); i++) {
                    JsonObject pointsInPoly = jA2.getJsonObject(i);
                    Point p = loadPoint(pointsInPoly);

                    ((PolygonObj) ret).addPoint(p);
                }
                
                break;
            default:
                System.out.println("Cannnot identify the object!!");
                break;
        }

        return ret;
    }

    private RenderInfo loadRenderInfo(JsonObject data) {
        double x = Double.parseDouble(data.get(JSON_INFO_RI_POS_X).toString());
        double y = Double.parseDouble(data.get(JSON_INFO_RI_POS_Y).toString());
        double h = Double.parseDouble(data.get(JSON_INFO_RI_H).toString());
        double l = Double.parseDouble(data.get(JSON_INFO_RI_L).toString());

        return new RenderInfo(x, y, l, h);
    }

    private Point loadPoint(JsonObject data) {
        RenderInfo ri = loadRenderInfo(data.getJsonObject(JSON_INFO_RI));

        return new Point(ri.getPosX(), ri.getPosY());
    }

    /**
     * This method exports the contents of the data manager to a code.
     *
     * @param data The data management component.
     *
     * @param filePath Path (including file name/extension) to where to export
     * the page to.
     *
     * @throws IOException Thrown should there be an error writing out data to
     * the file.
     */
    @Override
    public void exportData(AppDataComponent data, String filePath) throws IOException {
        ArrayList<DesignObj> dObjList = DataManager.getDObjList();

    }

    /**
     * This method is provided to satisfy the compiler, but it is not used by
     * this application.
     */
    @Override
    public void importData(AppDataComponent data, String filePath) throws IOException {
        // NOTE THAT THE Web Page Maker APPLICATION MAKES
        // NO USE OF THIS METHOD SINCE IT NEVER IMPORTS
        // EXPORTED WEB PAGES
        System.out.println("importData not supported");
    }
}
