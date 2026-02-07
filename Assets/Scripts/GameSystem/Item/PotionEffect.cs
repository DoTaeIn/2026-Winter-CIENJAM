using UnityEngine;

public class PotionEffect : ItemEffect
{
    public PotionEffect(BodyManager manager) : base(manager){}
    
    public override void DoEffect(float heal)
    {

        BodyPartType? randomType = bodyManager.GetRandomActiveBodyPart();

        if (randomType.HasValue) // 부위가 하나라도 살아있다면
        {
            // 2. 해당 타입의 BodyPart 객체를 찾습니다.
            BodyPart targetPart = bodyManager.GetPart(randomType.Value);

            // 3. 그 객체의 '이벤트'에 함수를 등록합니다.
            if (targetPart != null)
            {
                // [중요] 객체 자체가 아니라, 객체 안의 '이벤트'에 += 를 해야 합니다!
                targetPart.OnPartRestore += heal; 
            }
        }
    }
}
