/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.obj;

import javafx.scene.Node;
import javax.json.JsonObject;
import mg.EnumContainer.LINE_TYPE;
import mg.data.info.RenderInfo;
import mg.file.FileManager;
import mg.gui.LineSegRender;

/**
 *
 * @author YunJoonSoh
 */
public class LineSeg extends DesignObj {

    private static final RenderInfo DEFAULT_LINE_SEG_RI = new RenderInfo(0, 0, 0, 0);
    
    private Point start, end;
    private LINE_TYPE lineType;

    public LineSeg(LINE_TYPE lineType, double startX, double startY, double endX, double endY) {
        this.lineType = lineType;
    }
    
    public LineSeg(LINE_TYPE lt, Point start, Point end){
        this.lineType = lt;
        this.start = start;
        this.end = end;
    }

    /**
     * @return the start
     */
    public Point getStart() {
        return start;
    }

    /**
     * @param start the start to set
     */
    public void setStart(Point start) {
        this.start = start;
    }

    /**
     * @return the end
     */
    public Point getEnd() {
        return end;
    }

    /**
     * @param end the end to set
     */
    public void setEnd(Point end) {
        this.end = end;
    }

    /**
     * @return the lineType
     */
    public LINE_TYPE getLineType() {
        return lineType;
    }

    /**
     * @param lineType the lineType to set
     */
    public void setLineType(LINE_TYPE lineType) {
        this.lineType = lineType;
    }

    @Override
    public JsonObject toJsonObject() {
        return FileManager.makeDesignObjJsonObject(this);
    }

    @Override
    public Node getRenderObject() {
        if(this.start == null || this.end == null){
            System.out.println("Start or end is set to null in getRenderObj for lineseg");
        }
        shape = new LineSegRender(this);
        return shape;
    }

    @Override
    public boolean equals(Object that) {
        if (!(that instanceof LineSeg)) {
            return false;
        }

        LineSeg target = (LineSeg) that;

        if (!ri.equals(target.ri)) {
            System.out.println("RI Different");
            return false;
        }

        if (!start.equals(target.getStart())) {
            System.out.println("Start Different: " +  start.toString() + ", " + target.getStart().toString());
            return false;
        }

        if (!end.equals(target.getEnd())) {
            System.out.println("End Different");
            return false;
        }

        if (!lineType.equals(target.getLineType())) {
            System.out.println("LineType Different");
            return false;
        }

        return true;
    }
}
