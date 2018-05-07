/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.gui;

import javafx.beans.binding.Bindings;
import javafx.scene.shape.Circle;
import mg.data.obj.Point;

/**
 *
 * @author ysoh
 */
public class PointRender extends Circle {

    public PointRender(Point p) {
        setRadius(Point.DEFAULT_POINT_SIZE);

        translateXProperty().bindBidirectional(p.getRenderInfo().posXProperty());
        translateYProperty().bindBidirectional(p.getRenderInfo().posYProperty());

        Bindings.bindBidirectional(fillProperty(), p.getRenderInfo().colorProperty());

        setOnMousePressed(e -> {
            WorkspaceHandler.handleMousePressed(e, p);
        });

        setOnMouseDragged(e -> {
            WorkspaceHandler.handleMouseDraggedDesignObj(e, p);
        });

        setOnMouseEntered(e -> {
            WorkspaceHandler.handleMouseOver(e, p, true);
        });

        setOnMouseExited(e -> {
            WorkspaceHandler.handleMouseOver(e, p, false);
        });

//        setOnMouseClicked(e -> {
//            WorkspaceHandler.handleSelection(e, p);
//            e.consume();
//        });
    }
}
