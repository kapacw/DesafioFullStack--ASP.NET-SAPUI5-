sap.ui.define([
  "sap/ui/core/UIComponent",
  "sap/ui/Device",
  "./model/models"
], function (UIComponent, Device, models) {
  "use strict";

  return UIComponent.extend("tarefas.front.Component", {
    metadata: {
      manifest: "json",
      interfaces: ["sap.ui.core.IAsyncContentCreation"]
    },

    init: function () {
      // Chama o init da superclasse
      UIComponent.prototype.init.call(this);

      // Modelo de dispositivo
      this.setModel(models.createDeviceModel(), "device");

      // Inicializa o roteador
      this.getRouter().initialize();
    },

    getContentDensityClass: function () {
      if (this.contentDensityClass === undefined) {
        if (
          document.body.classList.contains("sapUiSizeCozy") ||
          document.body.classList.contains("sapUiSizeCompact")
        ) {
          this.contentDensityClass = "";
        } else if (!Device.support.touch) {
          this.contentDensityClass = "sapUiSizeCompact";
        } else {
          this.contentDensityClass = "sapUiSizeCozy";
        }
      }
      return this.contentDensityClass;
    }
  });
});
