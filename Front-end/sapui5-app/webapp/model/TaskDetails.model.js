sap.ui.define(["sap/ui/model/json/JSONModel"], function (JSONModel) {
  "use strict";

  return {
    createTaskDetailsModel: function () {
      const oModel = new JSONModel({
        id: null,
        title: "",
        completed: false,
        userId: null
      });
      
      oModel.setDefaultBindingMode("TwoWay"); // <--- IMPORTANTE!
      return oModel;
    }
  };
});
