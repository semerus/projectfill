/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.info;

import javafx.beans.property.DoubleProperty;
import javafx.beans.property.ObjectProperty;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.property.SimpleObjectProperty;
import javafx.scene.paint.Color;
import javafx.scene.paint.Paint;
import javax.json.JsonObject;
import mg.file.FileManager;

/**
 *
 * @author YunJoonSoh
 */
public class RenderInfo implements InfoObj {

    private final DoubleProperty posX, posY, length, height;
    private ObjectProperty<Paint> color;

    public RenderInfo(double posX, double posY, double length, double height) {
        this.posX = new SimpleDoubleProperty(posX);
        this.posY = new SimpleDoubleProperty(posY);
        this.length = new SimpleDoubleProperty(length);
        this.height = new SimpleDoubleProperty(height);
        this.color = new SimpleObjectProperty<>(Color.BLACK);
    }

    public DoubleProperty posXProperty() {
        return posX;
    }

    /**
     * @return the posX
     */
    public double getPosX() {
        return posX.get();
    }

    /**
     * @param posX the posX to set
     */
    public void setPosX(double posX) {
        this.posX.set(posX);
    }

    public DoubleProperty posYProperty() {
        return posY;
    }

    /**
     * @return the posY
     */
    public double getPosY() {
        return posY.get();
    }

    /**
     * @param posY the posY to set
     */
    public void setPosY(double posY) {
        this.posY.set(posY);
    }

    public DoubleProperty lengthProperty() {
        return length;
    }

    /**
     * @return the length
     */
    public double getLength() {
        return length.get();
    }

    /**
     * @param length the length to set
     */
    public void setLength(double length) {
        this.length.set(length);
    }

    public DoubleProperty heightProperty() {
        return height;
    }

    /**
     * @return the height
     */
    public double getHeight() {
        return height.get();
    }

    /**
     * @param height the height to set
     */
    public void setHeight(double height) {
        this.height.set(height);
    }

    @Override
    public String toString() {
        String ret = "";
        ret += "(X, Y)=" + getPosX() + ", " + getPosY() + ", length=" + getLength() + ", height" + getHeight() + ", color" + getColor();
        return ret;
    }

    @Override
    public String toDisplayString() {
        String ret = "";
        ret += "Position X: " + getPosX() + "\n";
        ret += "Position Y: " + getPosY() + "\n";
        ret += "Length    : " + getLength() + "\n";
        ret += "Height    : " + getHeight() + "\n";
        ret += "Color     : " + getColor() + "\n";

        return ret;
    }

    @Override
    public JsonObject toJsonObject() {
        return FileManager.makeRenderInfoObject(this);
    }

    @Override
    public String toCode() {
        throw new UnsupportedOperationException("Not supported, this should not get into code");
    }

    @Override
    public boolean equals(Object that) {
        if (!(that instanceof RenderInfo)) {
            return false;
        }

        RenderInfo target = (RenderInfo) that;

        if (getPosX() != target.getPosX()) {
            System.out.println("PosX differ");
            return false;
        }

        if (getPosY() != target.getPosY()) {
            System.out.println("PosY differ");
            return false;
        }

        if (getLength() != target.getLength()) {
            System.out.println("Length differ");
            return false;
        }

        if (getHeight() != target.getHeight()) {
            System.out.println("Height differ");
            return false;
        }

        return true;
    }

    public ObjectProperty colorProperty() {
        return color;
    }
    
    public Paint getColor(){
        return color.get();
    }
    

    /**
     * @param color the color to set
     */
    public void setColor(ObjectProperty<Paint> color) {
        this.color = color;
    }
}
