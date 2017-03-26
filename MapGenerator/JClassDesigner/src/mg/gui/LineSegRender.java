/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.gui;

import javafx.scene.Group;
import javafx.scene.shape.Line;
import mg.data.info.RenderInfo;
import mg.data.obj.LineSeg;

/**
 *
 * @author ysoh
 */
public class LineSegRender extends Group {

    public LineSegRender(LineSeg ls) {
        System.out.println("LineSeg getRenderObject called");

        Line oneLine = new Line();
        RenderInfo startInfo = ls.getStart().getRenderInfo();
        oneLine.startXProperty().bind(startInfo.posXProperty());
        oneLine.startYProperty().bind(startInfo.posYProperty());
        
        RenderInfo endInfo = ls.getEnd().getRenderInfo();
        oneLine.endXProperty().bind(endInfo.posXProperty());
        oneLine.endYProperty().bind(endInfo.posYProperty());
        
        oneLine.setStrokeWidth(3);
        getChildren().add(oneLine);

        setOnMouseClicked(e -> {
            WorkspaceHandler.handleSelection(e, ls);
            e.consume();
        });
    }
}
