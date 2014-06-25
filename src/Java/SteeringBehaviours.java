import com.jme3.math.Vector3f;
import com.jme3.math.Ray;
import com.jme3.scene.Node;
import com.jme3.scene.Spatial;

public final static class SteeringBehaviours{
    
    public static final float Random(float min, float max){
        max = (float) (max * Math.random());
        return min + max;
    }
    
    public static Vector3f Seek(Spatial self, Spatial target){
        return (target.getWorldTranslation().subtract(self.getWorldTranslation()).normalize());
    }
    
    public static Vector3f Seek(Vector3f self, Vector3f target){
        return (target.subtract(self).normalize());
    }
    
    public Vector3f Wander(Vector3f self){
        return new Vector3f(Random(0.0f,1.0f),Random(0.0f,1.0f),Random(0.0f,1.0f));
    }
    
    public static Vector3f Flee(Spatial self, Spatial target){
        return self.getWorldTranslation().subtract(target.getWorldTranslation()).normalize();
    }
    
    public static Vector3f Flee(Vector3f self, Vector3f target){
        return self.subtract(target).normalize();
    }
    
    public static Vector3f Avoid(Spatial self, Spatial[] others, float maxDistance){
        Vector3f dir = Vector3f.ZERO;
        Vector3f forward = self.getWorldTranslation().add(
        self.getWorldTransform().getRotation().toRotationMatrix().mult(new Vector3f(1,1,1)).normalize());
        Main.entityData.getComponent(id, Spatial)
        for(Spatial s : others)
            if(s.getWorldTranslation().distanceSquared(self.getWorldTranslation()) > maxDistance/2)
                forward.subtract(Vector3f.UNIT_XYZ.divide(s.getWorldTranslation()));
        return dir.normalize();
    }
}
