using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PalletMover : ManejoPallets {

    public MoveType miInput;
    public enum MoveType {
        WASD,
        Arrows,
        Joystick
    }
    public GameObject grab;
    bool shouldGrab = false;
    public GameObject hold;
    bool shouldHold = false;
    public GameObject deposit;
    bool shouldDeposit = false;
    public ManejoPallets Desde, Hasta;
    bool segundoCompleto = false;

    private void Update() {
        switch (miInput) {
            case MoveType.WASD:
                if (!Tenencia() && Desde.Tenencia() && Input.GetKeyDown(KeyCode.A)) {
                    PrimerPaso();
                }
                if (Tenencia() && Input.GetKeyDown(KeyCode.S)) {
                    SegundoPaso();
                }
                if (segundoCompleto && Tenencia() && Input.GetKeyDown(KeyCode.D)) {
                    TercerPaso();
                }
                break;
            case MoveType.Arrows:
                if (!Tenencia() && Desde.Tenencia() && Input.GetKeyDown(KeyCode.LeftArrow)) {
                    PrimerPaso();
                }
                if (Tenencia() && Input.GetKeyDown(KeyCode.DownArrow)) {
                    SegundoPaso();
                }
                if (segundoCompleto && Tenencia() && Input.GetKeyDown(KeyCode.RightArrow)) {
                    TercerPaso();
                }
                break;
            case MoveType.Joystick:
                if (!Tenencia() && Desde.Tenencia() && shouldGrab)
                {
                    PrimerPaso();
                    Debug.Log("Primer Paso");
                    shouldGrab = false;
                }
                if (Tenencia() && shouldHold)
                {
                    SegundoPaso();
                    shouldHold = false;
                }
                if (segundoCompleto && Tenencia() && shouldDeposit)
                {
                    TercerPaso();
                    shouldDeposit = false;
                }
                break;
            default:
                break;
        }
    }
    public void OnGrabButton() { shouldGrab = true; }
    public void OnHoldButton() { shouldHold = true; }
    public void OnDepotButton() { shouldDeposit = true; }

    public void PrimerPaso() {
        Desde.Dar(this);
        Debug.Log("First Step");
        segundoCompleto = false;
    }
    public void SegundoPaso() {
        base.Pallets[0].transform.position = transform.position;
        segundoCompleto = true;
    }
    public void TercerPaso() {
        Dar(Hasta);
        segundoCompleto = false;
    }

    public override void Dar(ManejoPallets receptor) {
        if (Tenencia()) {
            if (receptor.Recibir(Pallets[0])) {
                Pallets.RemoveAt(0);
            }
        }
    }
    public override bool Recibir(Pallet pallet) {
        if (!Tenencia()) {
            pallet.Portador = this.gameObject;
            base.Recibir(pallet);
            return true;
        }
        else
            return false;
    }
}
