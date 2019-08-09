

public class TankAIComponent {

    protected TankAI ai;
    public TankAI AI { get => ai; }

    public TankAIComponent(TankAI ai) {
        this.ai = ai;
    }

    public virtual void Update() {

    }
}
