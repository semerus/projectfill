/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package jcd.file;

import java.util.ArrayList;
import mg.JClassDesigner;
import mg.data.DataManager;
import mg.data.obj.DesignObj;
import mg.file.FileManager;
import org.junit.AfterClass;
import static org.junit.Assert.*;
import org.junit.BeforeClass;
import org.junit.FixMethodOrder;
import org.junit.Test;
import org.junit.runners.MethodSorters;
import saf.components.AppComponentsBuilder;
import saf.components.AppDataComponent;
import saf.components.AppFileComponent;
import static saf.settings.AppStartupConstants.PATH_WORK;

/**
 *
 * @author ysoh
 */
@FixMethodOrder(MethodSorters.NAME_ASCENDING)
public class FileManagerTest {

    static AppDataComponent dataComponent;
    static AppFileComponent fileComponent;
    final static boolean IS_STATIC = true;
    final static boolean IS_FINAL = true;

    static ArrayList<ArrayList<DesignObj>> listOfdObjList;

    @BeforeClass
    public static void setUpClass() throws Exception {
        System.out.println("FileManagerTest.java STARTED");

        JClassDesigner jcd = new JClassDesigner();
        AppComponentsBuilder builder;
        builder = jcd.makeAppBuilderHook();
        fileComponent = builder.buildFileComponent();
        dataComponent = builder.buildDataComponent();

        listOfdObjList = new ArrayList<>();

        ///////////////////////////////////////////////////////////////////////
        // zero dObjList: 1 DesignBox
        ArrayList<DesignObj> dObjList0 = objectList0();
        listOfdObjList.add(dObjList0);

        ///////////////////////////////////////////////////////////////////////
        // fifth dObjList: default package, default class
        ArrayList<DesignObj> dObjList5 = new ArrayList<>();
        DataManager.setdObjList(dObjList5);

        listOfdObjList.add(dObjList5);
    }

    private static ArrayList<DesignObj> objectList0() {
        ArrayList<DesignObj> dObjList0 = new ArrayList<>();
        DataManager.setdObjList(dObjList0);

        return dObjList0;
    }

    @AfterClass
    public static void tearDownClass() {
        System.out.println("FileManagerTest.java DONE");
    }

    @Test
    public void ASaveData() throws Exception {
        System.out.println("saveData");

        DataManager dm = (DataManager) dataComponent;
        FileManager instance = (FileManager) fileComponent;

        for (int i = 0; i < listOfdObjList.size(); i++) {
            System.out.println("\tThis is saveData Test for " + i + "th dObjList.");
            DataManager.setdObjList(listOfdObjList.get(i));
            instance.saveData(dm, PATH_WORK + "test" + i);
        }
    }

    @Test
    public void BLoadData() throws Exception {
        System.out.println("loadData");

        DataManager dm = (DataManager) dataComponent;
        FileManager fm = (FileManager) fileComponent;

        for (int i = 0; i < listOfdObjList.size(); i++) {
            System.out.println("\tThis is loadData Test for " + i + "th dObjList.");
            fm.loadData(dm, PATH_WORK + "test" + i);
            assertArrayEquals(listOfdObjList.get(i).toArray(), DataManager.getDObjList().toArray());
        }
    }
}
