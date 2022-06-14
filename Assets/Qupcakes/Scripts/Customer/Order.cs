//using System;

///*
// * Order container
// */

//public class Order
//{
//    public Cake cake { get; private set; }
//    public EntanglementStatus entanglementStatus { get; private set; }
//    public int EntanglementPartnerInd = -1;

//    // Constructor
//    public Order(GameCakeType cakeType)
//    {
//        cake = new Cake(cakeType);
//        entanglementStatus = EntanglementStatus.None;
//    }

//    public override string ToString()
//    {
//        return cake.ToString();
//    }

//    public GameCakeType GetGameCakeType()
//    {
//        return cake.GetCakeType();
//    }

//    public void SetEntanglementStatus(EntanglementStatus newStatus)
//    {
//        entanglementStatus = newStatus;
//    }
//}
